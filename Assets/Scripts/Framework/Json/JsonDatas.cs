using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sample
{
    public List<int> po;
    public List<int> value;
}

[System.Serializable]
public class SampleList
{
    public List<Sample> samples;
}

[System.Serializable]
public class TableData
{
    public string work;
    public List<int> startAndEnd;
    public SampleList sampleList;
}

[System.Serializable]
public class TableDataList
{
    public List<TableData> tables;
}
