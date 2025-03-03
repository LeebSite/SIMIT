﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pertamina.SIMIT.Application.Common.Exceptions;
using Pertamina.SIMIT.Application.Common.Mappings;
using Pertamina.SIMIT.Application.Services.Persistence;
using Pertamina.SIMIT.Application.Services.Storage;
using Pertamina.SIMIT.Domain.Entities;
using Pertamina.SIMIT.Shared.Mahasiswas.Constants;
using Pertamina.SIMIT.Shared.Mahasiswas.Queries.GetMahasiswa;

namespace Pertamina.SIMIT.Application.Mahasiswas.Queries.GetMahasiswa;
public class GetMahasiswaQuery : IRequest<GetMahasiswaResponse>
{
    public Guid MahasiswaId { get; set; }
}

public class GetMahasiswaResponseMapping : IMapFrom<Mahasiswa, GetMahasiswaResponse>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Mahasiswa, GetMahasiswaResponse>()
            .ForMember(dest => dest.LaporanId, opt => opt.MapFrom(src =>
                src.Laporans.FirstOrDefault() != null ? src.Laporans.FirstOrDefault().Id : (Guid?)null))
            .ForMember(dest => dest.LaporanDeskripsi, opt => opt.MapFrom(src =>
                src.Laporans.FirstOrDefault() != null ? src.Laporans.FirstOrDefault().Deskripsi : null))
            .ForMember(dest => dest.MahasiswaAttachmentId, opt => opt.MapFrom(src =>
               src.Attachments.FirstOrDefault() != null ? src.Attachments.FirstOrDefault().Id : (Guid?)null))
            .ForMember(dest => dest.FileName, opt => opt.MapFrom(src =>
               src.Attachments.FirstOrDefault() != null ? src.Attachments.FirstOrDefault().FileName : (string?)null))
            .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src =>
               src.Attachments.FirstOrDefault() != null ? src.Attachments.FirstOrDefault().FileContentType : (string?)null))
            .ForMember(dest => dest.Logbooks, opt => opt.MapFrom(src =>
                src.Logbooks.Select(lb => new LogbookItem
                {
                    LogbookId = lb.Id,
                    Aktifitas = lb.Aktifitas,
                    LogbookDate = lb.LogbookDate
                }).ToList()));
    }
}
public class GetMahasiswaQueryHandler : IRequestHandler<GetMahasiswaQuery, GetMahasiswaResponse>
{
    private readonly ISIMITDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStorageService _storageService;

    public GetMahasiswaQueryHandler(ISIMITDbContext context, IMapper mapper, IStorageService storageService)
    {
        _context = context;
        _mapper = mapper;
        _storageService = storageService;
    }

    public async Task<GetMahasiswaResponse> Handle(GetMahasiswaQuery request, CancellationToken cancellationToken)
    {
        // Ambil data mahasiswa dengan pemrosesan lainnya
        var mahasiswa = await _context.Mahasiswas
            .AsNoTracking()
            .Include(m => m.Pembimbing)
            .Include(m => m.Laporans)
            .Include(m => m.Attachments)
            .Include(m => m.Logbooks)
            .Where(x => !x.IsDeleted && x.Id == request.MahasiswaId)
            .SingleOrDefaultAsync(cancellationToken);

        // Jika mahasiswa atau attachment tidak ditemukan, lempar exception
        if (mahasiswa is null)
        {
            throw new NotFoundException(DisplayTextFor.Mahasiswa, request.MahasiswaId);
        }

        // Persiapkan data untuk response

        // Tambahkan data dari mahasiswa attachment ke dalam response
        //if (mahasiswa != null)
        //{

        //    mahasiswa.Content = await _storageService.ReadAsync(mahasiswaAttachment.StorageFileId);
        //}
        //var resp = new GetMahasiswaResponse
        //{
        //    MahasiswaAttachmentId = mahasiswaAttachment.

        //};
        var response = _mapper.Map<GetMahasiswaResponse>(mahasiswa);
        response.Content = await _storageService.ReadAsync(mahasiswa.Attachments.FirstOrDefault().StorageFileId);

        return response;
        ;
    }
}

