using System.Collections.Generic;
namespace Model.Content{

[System.Serializable]
public class ContentRoot {
    public List<ContentData> contents;
}


[System.Serializable]
public class ContentData {
    public int id;
    public string description;
    public string effect_name;
    public int effect;
    public List<string> dialogue;
    public List<CommentData> comments;
    public List<int> commentEffect;
    public int likeEffect;
}

[System.Serializable]
public class CommentData {
    public string name;
    public string comment;
}
}
