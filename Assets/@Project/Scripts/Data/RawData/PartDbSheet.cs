using System.Collections.Generic;

[ExcelAsset(AssetPath = "Resources/Data")]
public class PartDbSheet : BaseDbSheet<PartData>
{
    public List<PartData> Parts_SO;

    public override List<PartData> Entities => Parts_SO;
}
