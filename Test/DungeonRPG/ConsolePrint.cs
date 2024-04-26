using System.Drawing;

internal class ConsolePrint
{
    public static void Title()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(("________________________________________________________"));
        Console.WriteLine(("  _______ ________   _________   _____  _____   _____ "));
        Console.WriteLine((" |__   __|  ____\\ \\ / /__   __| |  __ \\|  __ \\ / ____|"));
        Console.WriteLine(("    | |  | |__   \\ V /   | |    | |__) | |__) | |  __ "));
        Console.WriteLine(("    | |  |  __|   > <    | |    |  _  /|  ___/| | |_ |"));
        Console.WriteLine(("    | |  | |____ / . \\   | |    | | \\ \\| |    | |__| |"));
        Console.WriteLine(("    |_|  |______/_/ \\_\\  |_|    |_|  \\_\\_|     \\_____|"));
        Console.WriteLine(("________________________________________________________"));
        Console.ResetColor();
        Console.WriteLine(("\n1. 시작하기"));
        Console.WriteLine(("0. 나가기\n"));
        
        switch (CheckInput(0,1))
        {
            case 0:
                Environment.Exit(0);//콘솔 종료
                break;
        }

    }
    //올바른 입력인지 확인
    public static int CheckInput(int _startNum, int _endNum)
    {
        Console.WriteLine("원하시는 행동을 입력해주세요");
        
        while (true)
        {
            Console.Write(">>");
            if (int.TryParse(Console.ReadLine(), out int answer) && answer>=_startNum&&answer<=_endNum )
            {
                return answer;
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
            }

        }

    }
}
