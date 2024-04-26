
public class DungenRPG
{
    public static void Main(string[] args)
    {
        GameManager gameManager = new GameManager();
        gameManager.GameStart();

    }
}

class GameManager
{
    Player player;
    Inventory inventory =new Inventory();
    Shop shop = new Shop();

    
    //게임매니저 생성자
    public GameManager()
    {
        Init();
    }
    void Init()
    {
        ConsolePrint.Title();

        string name = " ";
        string job = "";

        Console.Clear();
        Console.Write("사용하실 캐릭터의 이름을 적어주세요 >> ");
         name = Console.ReadLine();
        Console.Clear();
        Console.WriteLine("직업\n1. 전사 \n2. 마법사\n3. 궁수");
        Console.Write("시작하실 직업을 선택해주세요 >>");

        switch(ConsolePrint.CheckInput(1,3))
        {
            case 1:
                job = "전사";
                break;
            case 2:
                job = "마법사";
                break;
            case 3:
                job = "궁수";
                break;
        }
        player = new Player(1,name,job,10,5,100,1500);

    }
    public void GameStart()
    {
        Console.Clear();
        Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
        Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
        Console.WriteLine("1. 상태보기");
        Console.WriteLine("2. 인벤토리");
        Console.WriteLine("3. 상점");
        Console.WriteLine("4. 던전입장");
        Console.WriteLine("5. 휴식하기");
        Console.WriteLine();

        switch (ConsolePrint.CheckInput(1, 5))
        {
            case 1: //상태보기
                     Stat();
                break;
            case 2://인벤토리 열기
                    inventory.OpenInventory();
                break;
            case 3: //상점 열기
                     shop.EnterShop(ref player.gold, ref inventory.inventoryItem);
                break;
            case 4: //던전가기
                Dungeon();
                break;
            case 5: // 쉬기
                Rest();
                break;

        }
        GameStart();
    }

    void Stat()
    {
        Console.Clear();
        Console.WriteLine("상태 보기\n");
        Console.WriteLine($"Lv. {player.level}");
        Console.WriteLine($"{player.name} ({player.job})");

        //장비 장착시 출력 변경
        if (inventory.attackStat > 0)
        {
            Console.WriteLine("공격력 : {0} + ({1})", player.attack + inventory.attackStat, inventory.attackStat);
        }
        else
        {
            Console.WriteLine($"공격력 : {player.attack}");
        }
        if (inventory.defenseStat > 0)
        {
            Console.WriteLine("방어력 : {0} + ({1})", player.defense + inventory.defenseStat, inventory.defenseStat);
        }
        else
        {
            Console.WriteLine($"방어력 : {player.defense}");
        }

        Console.WriteLine($"체 력 : {player.health}");
        Console.WriteLine($"Gold : {player.gold}\n");
        Console.WriteLine("0. 나가기\n");
       

        ConsolePrint.CheckInput(0, 0);

    }
    void Rest()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("휴식");
        Console.ResetColor();
        Console.WriteLine("휴식을 하시면 500G 로 체력을 회복합니다.\n");
        Console.WriteLine("휴식을 하시겠습니까?\n1.네\n0.아니요\n");
       
        while(true)
        {
            if (ConsolePrint.CheckInput(0, 1) == 1)
            {
                if (player.gold < 500)
                {
                    Console.WriteLine("Gold 가 부족합니다.\n");
                  
                }
                else if( player.health >= 100)
                {
                    Console.WriteLine("체력이 다 찼기 때문에 더 이상 휴식하실 수 없습니다.\n");
                }
                else
                {
                    player.health += 10;
                    player.gold -= 500;
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"휴식을 완료했습니다. 현재 체력: {player.health} 남은 Gold: {player.gold}");
                    Console.ResetColor();
                }
            }
            else
            {
                break;
            }
            
        }
        
      
    }
     void CheckDungeonSucess(int _requireDef,int _reward)
    {
        Console.Clear();

        int ATK = player.attack + inventory.attackStat;
        int DEF = (int)player.defense +inventory.defenseStat;
        string Level = "";

        switch(_requireDef)
        {
            case 5:
                Level = "쉬운";
                break;
            case 11:
                Level = "일반";
                break;
            case 17:
                Level = "어려운";
                break;
        }

        Random random = new Random();
        int Sucess = random.Next(1, 100);


        if (DEF < _requireDef&&Sucess < 40)  //던전 실패
        {
          
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("던전 실패\n");
            Console.ResetColor();
            Console.WriteLine("더 강해지고 도전하시길 바랍니다.\n");
            Console.WriteLine("[탐험 결과]");
            Console.WriteLine($"체력 {player.health} -> {player.health/2}");

            player.health = player.health / 2;
        }
        else //던전 성공
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("던전 클리어\n");
            Console.ResetColor();
            Console.WriteLine("축하합니다!!\n{0} 던전 을 클리어 하였습니다.\n",Level);
            Console.WriteLine("[탐험 결과]");
            Console.WriteLine($"체력 {player.health} -> {player.health- random.Next(20 - (DEF - _requireDef), 35 - (DEF - _requireDef))}");
            Console.WriteLine($"Gold {player.gold}G -> {player.gold + _reward + random.Next(20 - (DEF - _requireDef), 35 - (DEF - _requireDef))}G");

            player.gold += _reward + (_reward / 100 * random.Next(ATK, 2 * ATK));//추가 보상 확률
            player.health -= random.Next(20 - (DEF - _requireDef), 35 - (DEF - _requireDef));//체력 감소
            player.level++;
            player.defense += 0.5f;
        }

        Console.WriteLine("\n0. 나가기\n");
        ConsolePrint.CheckInput(0,0);
    }
    void Dungeon()
    {
      
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("던전");
        Console.ResetColor();
        Console.WriteLine("이곳에서 들어갈 던전을 선택할 수 있습니다.\n");
        Console.WriteLine("1.쉬운 던전    | 방어력 5 이상 권장");
        Console.WriteLine("2.일반 던전    | 방어력 11 이상 권장");
        Console.WriteLine("3.어려운 던전  | 방어력 17 이상 권장\n");
        Console.WriteLine("0. 나가기\n");

        switch(ConsolePrint.CheckInput(0,3))
        {
            case 1:
                CheckDungeonSucess(5,1000);
                break;
            case 2:
                CheckDungeonSucess(11, 1700);
                break;
            case 3:
                CheckDungeonSucess(17, 2500);
                break;
        }

    }
}

class Inventory 
{
    public List <Item> inventoryItem = new List<Item>();
    string icon = "[E]"; //착용을 표시하기 위한 아이콘
    bool equip = false;  
    public int attackStat = 0;
    public int defenseStat = 0;

    //인벤토리창 출력
    void InvetoryDialogue()
    {
        Console.Clear();

        Console.WriteLine("인벤토리\n");
        Console.WriteLine("아이템 목록");
        for (int i = 0; i < inventoryItem.Count(); i++)
        {
            if (equip == true)
            {
                Console.Write($"-{i + 1} ");
                if (inventoryItem[i].itemEquip==true)
                {
                    Console.Write(icon);
                }
            }
            else
            {
                Console.Write("-");
                if (inventoryItem[i].itemEquip == true)
                {
                    Console.Write(icon);
                }
            }
            Console.WriteLine($"{inventoryItem[i].itemName} | {inventoryItem[i].itemType} +{inventoryItem[i].itemStat} | {inventoryItem[i].itemDescript}");

        }
        Console.WriteLine();

        if(equip == false)
        {
            Console.WriteLine("1. 장착 관리");
        }
        Console.WriteLine("0. 나가기\n");

    }
    //장비의 장착과 해제를 담당
    void ItemManagement(int _order)
    {
        InvetoryDialogue();

        if (inventoryItem[_order-1].itemEquip==false)//아이템 장착
        {
            inventoryItem[_order-1].itemEquip = true;

            if(inventoryItem[_order-1].itemType == "방어력")
            {
                defenseStat += inventoryItem[_order - 1].itemStat;
            }
            else //공격력
            {
                attackStat+= inventoryItem[_order-1].itemStat;
            }
        }
        else//아이템 해제
        {
            inventoryItem[_order - 1].itemEquip =false;

            if (inventoryItem[_order - 1].itemType == "방어력")
            {
                defenseStat -= inventoryItem[_order - 1].itemStat;
            }
            else //공격력
            {
                attackStat -= inventoryItem[_order - 1].itemStat;
            }
        }

        InvetoryDialogue();
    }
    //인벤토리 보여주고 행동 입력받기
    public void OpenInventory()
    {
        InvetoryDialogue();
        int answer = -1;

        if (equip == false)
        {
            answer = ConsolePrint.CheckInput(0, 1);

            if (answer == 1)
            {
                equip = true;
                OpenInventory();
            }
            else
            {
                equip = false;
            }
        }
        else if (equip == true)
        {
            while (true)
            {
                answer = ConsolePrint.CheckInput(0, inventoryItem.Count());
                if (answer == 0)
                {
                    equip = false;
                    OpenInventory();
                    break;
                }
                ItemManagement(answer);
            }
        }
    }
           
}
        
class Shop 
{
    List<Item> itmes = new List<Item>();

    bool buy = false;

    public Shop()
    {
        itmes.Add(new Item(false,"수련자의 갑옷","방어력",5,"수련에 도움을 주는 갑옷입니다.",1000));
        itmes.Add(new Item(false,"무쇠갑옷", "방어력",9, "무쇠로 만들어져 튼튼한 갑옷입니다.",100));
        itmes.Add(new Item(false,"스파르타의 갑옷", "방어력", 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.",3500));
        itmes.Add(new Item(false,"낡은 검", "공격력", 2, "쉽게 볼 수 있는 낡은 검 입니다.",600));
        itmes.Add(new Item(false,"청동 도끼", "공격력", 5, "어디선가 사용됐던거 같은 도끼입니다.",1500));
        itmes.Add(new Item(false,"스파르타의 창", "공격력", 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.",200));
        itmes.Add(new Item(false,"공포의 C#", "공격력", 333, "함부로 손댔다가는 쓴맛을 맛봅니다.",999));
    }

    //아이템 구매
    void BuyItem(ref int _Gold,int _answer, ref List<Item> inven)
    {
        if (itmes[_answer - 1].itemBuy == true)
        {
            Console.WriteLine("이미 구매한 아이템입니다.");
        }
        else if(itmes[_answer - 1].itemPrice > _Gold)
        {
            Console.WriteLine("돈이 부족합니다.");
        }
        else
        {
            _Gold -= itmes[_answer - 1].itemPrice; //소유한 금액에서 아이템 금액 빼기
            itmes[_answer - 1].itemBuy = true;      // 아이템을 구매했다고 표시


            inven.Add(itmes[_answer - 1]);

            ShopDialogue(ref _Gold,ref inven );                              //화면에 구매했다고 출력
            Console.WriteLine("아이템을 구매하셨습니다.");
        }
               
    }
    //상점 대사 출력
    void ShopDialogue(ref int _Gold, ref List<Item> inven)
    {
        Console.Clear();
        Console.WriteLine("상점\n");
        Console.WriteLine($"[보유 골드]\n{_Gold}G\n");
        Console.WriteLine("[아이템목록]");
        for (int i = 0; i < itmes.Count(); i++)
        {
            if (buy == true)
            {
                Console.Write($"-{i + 1} ");
            }
            else
            {
                Console.Write("-");
            }
            if (itmes[i].itemBuy == true)
            {
                Console.WriteLine($"{itmes[i].itemName} | {itmes[i].itemType} +{itmes[i].itemStat} | {itmes[i].itemDescript} | 구매완료");
            }
            else
            {
                Console.WriteLine($"{itmes[i].itemName} | {itmes[i].itemType} +{itmes[i].itemStat} | {itmes[i].itemDescript} | {itmes[i].itemPrice}G");
            }
        }
        Console.WriteLine();

        if (buy == false)
            Console.WriteLine("1. 아이템구매");

        Console.WriteLine("0. 나가기\n");
     

    }
    //상점에 들어와서 다음 행동 입력받기
    public void EnterShop(ref int _Gold,ref List<Item> inven)
    {
        ShopDialogue(ref _Gold, ref inven);
        int answer = -1 ;

        if(buy==false)
        {
            answer = ConsolePrint.CheckInput(0, 1);

            if (answer == 1)
            {
                buy = true;
                EnterShop(ref _Gold,ref inven);
            }
            else
            {
                buy = false;
            }
        }
        else if(buy == true)
        {
            while (true)
            {
                answer = ConsolePrint.CheckInput(0, itmes.Count());
                if(answer==0)
                {
                    buy = false;
                    EnterShop(ref _Gold, ref inven);
                    break;
                }
                BuyItem(ref _Gold, answer, ref inven);
            }
        }
    }
}
   

