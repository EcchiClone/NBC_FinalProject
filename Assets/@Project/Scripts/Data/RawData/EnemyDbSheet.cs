using System.Collections.Generic;

[ExcelAsset(AssetPath = "Resources/Data")]
public class EnemyDbSheet : BaseDbSheet<EnemyData>
{
	public List<EnemyData> Enemy_SO;

    public override List<EnemyData> Entities => Enemy_SO;
}
