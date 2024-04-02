namespace EasyEncounters.ViewModels;

public class SpellSlotViewModel
{
    public SpellSlotViewModel(int[] spellSlots)
    {
        SpellSlots = new Dictionary<int, int>();
        for(int i=0;i<spellSlots.Length; i++)
        {
            SpellSlots[i + 1] = spellSlots[i];
        }
    }

    public int EighthLevel
    {
        get => TryGet(8);
        set => TrySet(8, value);
    }

    public int FifthLevel
    {
        get => TryGet(5);
        set => TrySet(5, value);
    }

    public int FirstLevel
    {
        get => TryGet(1);
        set => TrySet(1, value);
    }

    public int FourthLevel
    {
        get => TryGet(4);
        set => TrySet(4, value);
    }

    public int NinthLevel
    {
        get => TryGet(9);
        set => TrySet(9, value);
    }

    public int SecondLevel
    {
        get => TryGet(2);
        set => TrySet(2, value);
    }

    public int SeventhLevel
    {
        get => TryGet(7);
        set => TrySet(7, value);
    }

    public int SixthLevel
    {
        get => TryGet(6);
        set => TrySet(6, value);
    }

    public Dictionary<int, int> SpellSlots
    {
        get; private set;
    }

    public int ThirdLevel
    {
        get => TryGet(3);
        set => TrySet(3, value);
    }

    private int TryGet(int size)
    {
        if (SpellSlots.ContainsKey(size))
            return SpellSlots[size];
        else
            return 0;
    }

    private void TrySet(int size, int count)
    {
        if (count == 0 && SpellSlots.ContainsKey(size))
            SpellSlots.Remove(size);
        else
            SpellSlots[size] = count;
    }
}