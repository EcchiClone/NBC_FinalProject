using System.Collections.Generic;

[ExcelAsset(AssetPath = "Resources/Data")]
public class TutorialDbSheet : BaseDbSheet<TutorialData>
{
	public List<TutorialData> Scripts_SO;

    public override List<TutorialData> Entities => Scripts_SO;
}
