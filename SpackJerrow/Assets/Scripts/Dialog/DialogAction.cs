using System;

public class DialogAction 
{
    
    public DialogAction(Action a, string s)
    {
        OnSelect = a;
        btnText = s;
    }

    public Action OnSelect { get; private set; }

    public string btnText { get; private set; }

}
