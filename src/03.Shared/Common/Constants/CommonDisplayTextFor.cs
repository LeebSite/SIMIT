using Pertamina.SIMIT.Shared.Common.Extensions;

namespace Pertamina.SIMIT.Shared.Common.Constants;

public static class CommonDisplayTextFor
{
    public const string Home = nameof(Home);
    public const string About = nameof(About);
    public const string Index = nameof(Index);
    public static readonly string MasterData = nameof(MasterData).SplitWords();

    public const string Id = "ID";
    public const string On = nameof(On);

    public const string Action = nameof(Action);
    public const string Required = nameof(Required);

    public const string Submit = nameof(Submit);
    public const string Submitted = nameof(Submitted);

    public const string Create = nameof(Create);
    public const string Created = nameof(Created);

    public const string Add = nameof(Add);
    public const string Added = nameof(Added);

    public const string Commit = nameof(Commit);
    public const string Committed = nameof(Committed);

    public const string Edit = nameof(Edit);
    public const string Edited = nameof(Edited);

    public const string Modify = nameof(Modify);
    public const string Modified = nameof(Modified);

    public const string Update = nameof(Update);
    public const string Updated = nameof(Updated);

    public const string Refresh = nameof(Refresh);
    public const string Refreshed = nameof(Refreshed);

    public const string Reload = nameof(Reload);
    public const string Reloaded = nameof(Reloaded);

    public const string Enable = nameof(Enable);
    public const string Enabled = nameof(Enabled);

    public const string Disable = nameof(Disable);
    public const string Disabled = nameof(Disabled);

    public const string Delete = nameof(Delete);
    public const string Deleted = nameof(Deleted);

    public const string Select = nameof(Select);
    public const string Selected = nameof(Selected);

    public const string Download = nameof(Download);
    public const string Downloaded = nameof(Downloaded);

    public const string Upload = nameof(Upload);
    public const string Uploaded = nameof(Uploaded);

    public const string Import = nameof(Import);
    public const string Imported = nameof(Imported);

    public const string Export = nameof(Export);
    public const string Exported = nameof(Exported);

    public const string View = nameof(View);
    public const string Search = nameof(Search);
    public const string Filter = nameof(Filter);
    public const string Apply = nameof(Apply);
    public const string Confirm = nameof(Confirm);
    public const string Cancel = nameof(Cancel);
    public const string Dismiss = nameof(Dismiss);
    public const string Get = nameof(Get);

    public const string By = nameof(By);
    public static readonly string CreatedBy = nameof(CreatedBy).SplitWords();
    public static readonly string ModifiedBy = nameof(ModifiedBy).SplitWords();

    public const string File = nameof(File);
    public static readonly string FileName = nameof(FileName).SplitWords();
    public static readonly string FileSize = nameof(FileSize).SplitWords();

    public static readonly string GeneralInfo = nameof(GeneralInfo).SplitWords();
    public const string Tables = nameof(Tables);
    public const string Charts = nameof(Charts);

    public const string Unsupported = nameof(Unsupported);
    public const string Service = nameof(Service);
    public const string Error = nameof(Error);
}
