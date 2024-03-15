using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class XMLManager : MonoBehaviour
{
    // 파일 경로와 이름
    private string _filePath = "Assets/@4_CMG/Resources/XML";
    private string _fileName = "perkTest.xml";

    // 퍼크의 전체 노드의 집합
    private List<List<string>> _nodes = new List<List<string>>();

    private void Start()
    {
        // SavePerkToXML();
    }

    private void SavePerkToXML()
    {
        // Xml 선언(버전, 인코딩 방식 설정)
        XmlDocument doc = new XmlDocument();
        doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", "no"));

        // 문서 root 요소 정의
        XmlElement root = doc.CreateElement("root"); // root
        doc.AppendChild(root);








    }

}
