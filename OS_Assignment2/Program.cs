namespace OS_Assignment2;

class Thread_safe_buffer
{
    static int Capacity = 10;
    static int[] TSBuffer = new int[Capacity];
    static int Front = 0;
    static int Back = 0;
    static int Count = 0;
    static bool enQueueFlag = false;
    static bool deQueueFlag = false;
    static object enqueueObj = new object();
    static object dequeueObj = new object();

    static void EnQueue(int eq)
    {
        while (true)
        {

            while (enQueueFlag)
            { 
                Thread.Sleep(10);
            }
            lock (enqueueObj)
            {
                if (!enQueueFlag)
                {
                    enQueueFlag = true;
                    break;
                }
                
            }
        }

        if (Count + 1 >= Capacity)
        {
            int newCapacity = Capacity * 2;
            var newBuffer = new int[newCapacity];
            for (int i = 0, j = Front; j != Back; i++) 
            {
                newBuffer[i] = TSBuffer[j];
                j = ++j == Capacity ? 0 : j;
            }
            Front = 0;
            Back = Count;
            Capacity = newCapacity;
            TSBuffer = newBuffer;
        }
        TSBuffer[Back] = eq;
        Back = (++Back >= Capacity) ? 0 : Back;
        Count += 1;
        enQueueFlag = false;
    }

    static int DeQueue()
    {
        while (true)
        {

            while (deQueueFlag)
            {
                Thread.Sleep(10);
            }
            lock (dequeueObj)
            {
                if (!deQueueFlag)
                {
                    deQueueFlag = true;
                    break;
                }

            }
        }
        int x = 0;
        x = TSBuffer[Front];
        if(Count > 0)
        {
            Front = ++Front==Capacity?0:Front;
            Count -= 1;
            deQueueFlag = false;
            return x;
        }
        else
        {
            deQueueFlag = false;
            //ADD EXCEPTION OR SOMETHING
            return 0;
        }
    }

    static void th01()
    {
        int i;

        for (i = 1; i < 51; i++)
        {
            EnQueue(i);
            Thread.Sleep(5);
        }
    }

    static void th011()
    {
        int i;

        for (i = 100; i < 151; i++)
        {
            EnQueue(i);
            Thread.Sleep(5);
        }
    }


    static void th02(object t)
    {
        int i;
        int j;

        for (i = 0; i < 60; i++)
        {
            j = DeQueue();
            Console.WriteLine("j={0}, thread:{1}", j, t);
            Thread.Sleep(100);
        }
    }
    static void Main(string[] args)
    {
        Thread t1 = new Thread(th01);
        Thread t11 = new Thread(th011);
        Thread t2 = new Thread(th02);
        Thread t21 = new Thread(th02);
        Thread t22 = new Thread(th02);

        t1.Start();
        t11.Start();
        t1.Join();
        t11.Join();
        t2.Start(1);
        t21.Start(2);
        t22.Start(3);
    }
}

