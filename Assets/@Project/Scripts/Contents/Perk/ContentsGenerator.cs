using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentsGenerator : MonoBehaviour
{
    private JsonSaveNLoader _json;

    private ContentList _emptyContentList = new ContentList();

    private void Awake()
    {
        _json = GetComponent<JsonSaveNLoader>();
        _emptyContentList.data = new List<ContentInfo>();
    }

    private void Start()
    {
        SaveEmptyContentFile(PerkTier.TIER1);
        SaveEmptyContentFile(PerkTier.TIER2);
        SaveEmptyContentFile(PerkTier.TIER3);
    }

    private void MakeContents(PerkTier tier)
    {
        _emptyContentList = new ContentList();
        _emptyContentList.data = new List<ContentInfo>();

        _emptyContentList.contentTier = tier;

        for (int i = 0; i < 8*(int)tier; i++)
        {
            ContentInfo emptyContent = new ContentInfo();
            emptyContent.contentIdx = i;
            emptyContent.name = $"EmptyPerk{i+1}Name";
            emptyContent.description = $"EmptyPerk{i+1}Description";
            _emptyContentList.data.Add(emptyContent);
        }

    }

    private void SaveEmptyContentFile(PerkTier tier)
    {
        MakeContents(tier);
        _json.SaveContentData(_emptyContentList, $"tier{(int)tier}ContentData");
    }
}
