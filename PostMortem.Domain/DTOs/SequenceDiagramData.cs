namespace PostMortem.Domain.DTOs;

public class Actor
{
    public string Name { get; set; }
}

public class Lifeline
{
    public Actor Actor { get; set; }
}

public class Message
{
    public string Text { get; set; }
    public Lifeline Sender { get; set; }
    public Lifeline Receiver { get; set; }
}