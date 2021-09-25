#r "nuget: Akka.FSharp"
#r "nuget: Akka.Remote"
#r "nuget: Akka.TestKit" 
#time "on"

open System
open System.Security.Cryptography
open System.Collections.Generic
open Akka.Actor
open Akka.Configuration
open Akka.FSharp
open Akka.TestKit


let configuration = 
    ConfigurationFactory.ParseString(
        @"akka {
            log-config-on-start : on
            stdout-loglevel : DEBUG
            loglevel : ERROR
            actor {
                provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
                debug : {
                    receive : on
                    autoreceive : on
                    lifecycle : on
                    event-stream : on
                    unhandled : on
                }
            }
            remote {
                helios.tcp {
                    port = 9000
                    hostname = localhost
                }
            }
        }")

let system = ActorSystem.Create ("romoteSystem", configuration)

type taskInfo = {
    actorID :int
    zerofind:int 
}

type env  = {
    env2 : list<string * string>}

let prefix = "wuyizhe；"
let key = int 4
let chars =  "0123456789abcdefghigklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"

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

   

     while count <3 do
        for str in newstring do 
            let mutable data = System.Text.Encoding.ASCII.GetBytes(str)
            let sha256 =SHA256Managed.Create()
            let mutable trans = sha256.ComputeHash(data)
            let mutable a =BitConverter.ToString(trans).Replace("-","").ToLower()
            let mutable zero= int 0
            
            if (findzeros a res)= true then list2 <-(str,a)::list2


        let mutable temp = []
        
        
        //newstring <-[]

        for str in newstring do
           temp <- (iteraStr str)@temp
        newstring <- temp
        count <- count+1

     if list2.IsEmpty |> not then
        printfn "%A"  list2
     list2
    
let Worker (mailbox: Actor<_>) =
    let rec loop()=actor {
        let! message = mailbox.Receive() ; 
        let sender = mailbox.Sender() 
        let mutable list1 = []

        printfn " recive message! %A" message

        match box message with 
            | :? taskInfo as recie ->
                 printfn " recive message! %A" message

                 let env1: env = {
                     env2 = findkey(recie.zerofind)(recie.actorID);
                 }

                 sender <! env1
                 sender <! "Done"
                 
            | _ -> failwith "unkonwn message"
        return! loop()
    }
    loop()

spawn system "worker" Worker

System.Console.ReadLine()  
system.Terminate() |> ignore