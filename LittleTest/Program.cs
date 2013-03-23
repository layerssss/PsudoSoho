using System;
using System.Runtime;


using System.Collections.Generic;







public class Program
{
    public static void Main()
    {
        var dog = new Dog();
        MakeItBark(dog);
        PlayWithIt(dog);
    }
    public static void MakeItBark(Barkable b)
    {
        b.Bark();
    }
    public static void PlayWithIt(Playable p)
    {
        p.Play();
    }
}
public interface Playable
{
    void Play();
}
public interface Barkable
{
    void Bark();
}






public class Dog:Playable,Barkable
{


    #region Barkable 成员

    public void Bark()
    {
    }

    #endregion


    #region Playable 成员

    public void Play()
    {
        throw new NotImplementedException();
    }

    #endregion
}
public class Tiger:Barkable
{
    #region Barkable 成员

    public void Bark()
    {
        throw new NotImplementedException();
    }

    #endregion
}
public class Toy:Playable 
{
    #region Playable 成员

    public void Play()
    {
        throw new NotImplementedException();
    }

    #endregion
}