#time "on"
//#load "Bootstrap.fsx"
#r "nuget: Akka.Remote"
#r "nuget: Akka.FSharp" 
#r "nuget: Akka.TestKit" 

open System
open System.Security.Cryptography
open System.Collections.Generic
open Akka.Actor
open Akka.Configuration
open Akka.FSharp
open Akka.TestKit


type bitmin={n:int;id:int}
let args: string array= fsi.CommandLineArgs
let N=args.[1]|>int 
let id = 1 

let prefix = "wuyizhe；"
let key = int 4
let chars =  "0123456789abcdefghigklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"

let generateX()=(System.Random()).Next(0,20)
let generateY x = (System.Random()).Next(x,62)
let generateXY() = 
    let x = generateX();
    let y = generateY x;
    (x,y)

let iteraStr(stri:string)= 

    let mutable list1 = []
    let mutable pre = stri

    
    
    //let mutable _start =  float(start/total)
    //_start<- floor (float(62) * _start)
   // let mutable _end = float (start+1/total)
   // _end <- ceil (float(62) * _end)
    
    for i in 0 .. 62 do
        let mutable backfix =""
        let mutable remind = i%62
        backfix<- string(chars.[int(remind)])+backfix
        list1 <- (pre+backfix)::list1

    list1

let findzeros(s:string)(x:int)=
    let mutable flag = true
    for i in 0..x do
        if not(s.[i].Equals('0'))then
            flag<-false
            
    flag

let findkey (n:int)(start:int)= 
     let res = n-1
     let mutable list2 =[]
     let mutable count = 1 
     let mutable _prefix = prefix+string(chars.[start-1])
     let mutable newstring = iteraStr _prefix 

   

     while count <5 do
        for str in newstring do 
            let mutable data = System.Text.Encoding.ASCII.GetBytes(str)
            let sha256 =SHA256Managed.Create()
            let mutable trans = sha256.ComputeHash(data)
            let mutable a =BitConverter.ToString(trans).Replace("-","").ToLower()
            let mutable zero= int 0
            
            if (findzeros a res)= true then list2 <-(str,a)::list2

        if list2.IsEmpty |> not then
            printfn "%A"  list2
        let mutable temp = []
        
        
        //newstring <-[]

        for str in newstring do
           temp <- (iteraStr str)@temp
        newstring <- temp
        count <- count+1

       
           
       
        

     if list2.IsEmpty |> not then
        printfn "%A"  list2
    
let system = ActorSystem.Create("Project1") 

let act_num =62


type Worker(name) = 
    inherit Actor()
    override x.OnReceive message = 
        match box message with 
        | :? bitmin as mine ->
                findkey (mine.n)(mine.id)
        | _ -> failwith "unKnown message"

type server(name) = 
    inherit Actor() 
    override x.OnReceive message=
        let sender = x.Sender

        match box message with 
        | :?  bitmin as mine ->
            let works = 
                [1..act_num]
                |> List.map(fun id -> system.ActorOf(Props(typedefof<Worker>,[|string(id) :> obj|])))
            for  i = 1 to act_num do 
                let workmessage = {bitmin.n = mine.n;bitmin.id=i}
                works.Item(i-1) <!workmessage 

        | :? string -> 
                sender <! PoisonPill.Instance
        | _ -> failwith "unKnown message"
                    

let Message = {bitmin.n = N;bitmin.id =id}
let the_server = system.ActorOf(Props(typeof<server>, [|string("Boss"):> obj|]))

the_server <! Message
the_server <! PoisonPill.Instance


Threading.Thread.Sleep(1)

system.Terminate().Wait()