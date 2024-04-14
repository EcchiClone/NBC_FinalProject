using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset(AssetPath = "Resources/Data")]
public class LevelDbSheet : BaseDbSheet<LevelData>
{
	public List<LevelData> Level_SO;

    public override List<LevelData> Entities => Level_SO;
}
