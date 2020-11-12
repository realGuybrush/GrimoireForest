using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class Talk
{
    public TextAsset inputText;
	private List<Sentence> Phrases = new List<Sentence>();//this contains all conversation of this item with player;
	//player will take sentences from clusters and use them, each sentence will cause event to happen, which will trigger the activation of other sentence of this item

    public void Load()
    {
		//CurrentSentence = 0;
        int evNum = 0;
        //List<Sentence> Phrases = new List<Sentence>();
        string [] arrayStr = inputText.text.Split('\n');
        for (int i = 0; i < arrayStr.Length; i++)
        {
            string[] arrayParts = arrayStr[i].Split('~');
            Phrases.Add(new Sentence());
            Phrases[Phrases.Count - 1].index = System.Convert.ToInt32(arrayParts[0]);
            Phrases[Phrases.Count - 1].changing_to = System.Convert.ToInt32(arrayParts[1]);
            Phrases[Phrases.Count - 1].action = arrayParts[2][0];
            Phrases[Phrases.Count - 1].disappear_after_use = (arrayParts[3][0] == '1');
            Phrases[Phrases.Count - 1].enabled = (arrayParts[4][0] == '1');
            evNum = System.Convert.ToInt32(arrayParts[5]);
            for (int k = 0; k < evNum; k++)
            {
                Phrases[Phrases.Count - 1].eve.Add(new Events(arrayParts[6 + k * 12],
                                                              arrayParts[7 + k * 12][0] == '1',
                                                              arrayParts[8 + k * 12][0] == '1',
                                                              arrayParts[9 + k * 12][0],
                                                              System.Convert.ToInt32(arrayParts[10 + k * 12]),
                                                              System.Convert.ToInt32(arrayParts[11 + k * 12]),
                                                              System.Convert.ToInt32(arrayParts[12 + k * 12]),
                                                              arrayParts[13 + k * 12],
                                                              (float)System.Convert.ToDouble(arrayParts[14 + k * 12]),
                                                              new Vector3((float)System.Convert.ToDouble(arrayParts[15 + k * 12]),
                                                                          (float)System.Convert.ToDouble(arrayParts[16 + k * 12]),
                                                                          (float)System.Convert.ToDouble(arrayParts[17 + k * 12]))));
            }
            Phrases[Phrases.Count - 1].BeautifulLine = arrayParts[arrayParts.Length - 1];
        }
    }
}
[System.Serializable]
public class Sentence
{
	//now, this points of data make a 
	public string BeautifulLine;
	//public bool change_to_another_cluster_or_line;//true - cluster, false - line
	public int changing_to;//to which cluster/line we are changing
    public int index; //index of sentence and or possible answers, there might be several lines with same index
    public char action;//' '-no, 't'-trade, 'e'-exit
	public List<Events> eve = new List<Events>();//event, that occurs after this phrase, comes before any assigned line change
	public bool disappear_after_use;
	public bool enabled;
}
/*[System.Serializable]
public class SentenceCluster
{
	public List<Sentence> Sentences = new List<Sentence>();//contains all sentences, that are in this cluster, this is needed in cases, when line leads to other line, but don't prevent player from taking other talking lines

	public int EnabledCount()
	{//fix? probably make it working 1 time at start, and then recalculate every time, when parameter enabled is changed
		int count = 0;
		for(int i=0; i< Sentences.Count; i++)
		{
			if(Sentences[i].enabled)
				count++;
		}
		return count;
	}

	public Sentence EnabledSentences(int index)
	{
		int j=-1;
		for(int i=0; i< Sentences.Count; i++)
		{
			if(Sentences[i].enabled)
				j++;
			if(j==index)
				return Sentences[i];
		}
		return null;
	}

}*/
