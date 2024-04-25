using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Authentication;
using static DungeonRPG.IDK;

namespace DungeonRPG
{
    internal interface IDK
    {
        static void Main(string[] args)
        {

            Player player = new Player();
            Shop shop = new Shop();
            Inventory inventory = new Inventory();

            //이름설정
            Console.Write("사용하실 플레이어 이름을 입력해주세요: ");
            player.name = Console.ReadLine();

            //직업설정
            Console.Clear();
            Console.Write("시작하실 직업을 선택해주세요:\n0.전사 \n1.마법사 \n2. 궁수\n ");

            int job = CheckInput(0, 2);
            switch (job)//직업에 따라 초기 스탯 변경해도 좋을것 같다.(미구현)
            {
                case 0:
                    player.job = "전사";
                    break;
                case 1:
                    player.job = "마법사";
                    break;
                case 2:
                    player.job = "궁수";
                    break;
            }

            //게임시작 
            while (true)
            {
                Console.Clear();
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
                Console.WriteLine("\n1. 상태보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");

                //잘못된 입력인지 출력
                int answer = CheckInput(1, 3);

                switch (answer)
                {
                    case 1: //상태보기
                        player.ShowStat(inventory.attackStat, inventory.defenseStat);
                        break;
                    case 2://인벤토리 열기
                        inventory.OpenInventory();
                        break;
                    case 3: //상점 열기
                        shop.EnterShop(ref player.gold, ref inventory.inventoryItem);
                        break;

                }
            }
        }

        class Player
        {
             int level = 01;  
             public string name="말하는감자";
             public string job = "";
             int attack=10;  
             int defense=5;  
             int health=100;  
             public int gold=1500;  


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
                    Console.WriteLine("방어력 : {0} + ({1})",defense+_addDefense,_addDefense);
                }
                else
                {
                    Console.WriteLine($"방어력 : {defense}");
                }

                Console.WriteLine($"체 력 : {health}");
                Console.WriteLine($"Gold : {gold}\n");
                Console.WriteLine("0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요");

                CheckInput(0,0);

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
                Console.WriteLine("원하시는 행동을 입력해주세요");

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
                    answer = CheckInput(0, 1);

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
                        answer = CheckInput(0, inventoryItem.Count());
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
                Console.WriteLine("원하시는 행동을 입력해주세요");

            }
            //상점에 들어와서 다음 행동 입력받기
            public void EnterShop(ref int _Gold,ref List<Item> inven)
            {
                ShopDialogue(ref _Gold, ref inven);
                int answer = -1 ;

                if(buy==false)
                {
                    answer = CheckInput(0, 1);

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
                        answer = CheckInput(0, itmes.Count());
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
   
       
        //아이템 클래스
        class Item
        {
            public bool itemEquip = false;
            public bool itemBuy { get; set; }
            public string itemName { get;  set; }
            public string itemType { get;  set; }
            public int itemStat { get; set; }
            public string itemDescript { get; set; }
            public int itemPrice {  get; set; }    


            public Item(bool _itemBuy, string _itemName,string _itemType, int _itemStat, string _itemDescript,int _itemPrice)
            {
                itemBuy = _itemBuy;
                itemName = _itemName;
                itemType = _itemType;
                itemStat = _itemStat;
                itemDescript = _itemDescript;
                itemPrice = _itemPrice;
            }
             
        }
      
       
     //올바른 입력인지 확인
        static public int CheckInput(int _startNum, int _endNum)
        {
            int answer = 0;
            while (true)
            {
                try
                {
                    Console.Write(">> ");
                    answer = int.Parse(Console.ReadLine());

                    for (int i = _startNum; i <= _endNum; i++) //입력이 숫자이지만 지정범위 내에 숫자가 아닐 경우
                    {
                        if (answer == i)
                        {
                            return answer;
                        }

                    }
                    Console.WriteLine("잘못된 입력입니다.");

                }
                catch (FormatException ex) //입력이 숫자가 아닐때 발생하는 오류
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }

              
            }

        }
    }



}
