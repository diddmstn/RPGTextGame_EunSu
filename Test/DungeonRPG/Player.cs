internal class Player
{
     public int level { get; set; }
    public string name = "(NoName)";
    public string job { get; set; }
    public int attack { get; set; }
    public float defense { get; set; }
    public int health { get; set; }
    public int gold = 1500;

    public Player(int level, string name, string job, int attack, int defense, int health, int gold)
    {
        this.level = level;
        this.name = name;
        this.job = job;
        this.attack = attack;
        this.defense = defense;
        this.health = health;
        this.gold = gold;
    }

    //플레이어의 스탯을 출력
    public void ShowStat(int _addAttack, int _addDefense)
    {
        Console.Clear();
        Console.WriteLine("상태 보기\n");
        Console.WriteLine($"Lv. {level}");
        Console.WriteLine($"{name} ({job})");

        //장비 장착시 출력 변경
        if (_addAttack > 0)
        {
            Console.WriteLine("공격력 : {0} + ({1})", attack + _addAttack, _addAttack);
        }
        else
        {
            Console.WriteLine($"공격력 : {attack}");
        }
        if (_addDefense > 0)
        {
            Console.WriteLine("방어력 : {0} + ({1})", defense + _addDefense, _addDefense);
        }
        else
        {
            Console.WriteLine($"방어력 : {defense}");
        }

        Console.WriteLine($"체 력 : {health}");
        Console.WriteLine($"Gold : {gold}\n");
        Console.WriteLine("0. 나가기\n");
        Console.WriteLine("원하시는 행동을 입력해주세요");

        ConsolePrint.CheckInput(0, 0);

    }
}
