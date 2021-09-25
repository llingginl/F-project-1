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
                    hostname = 192.168.0.131
                }
            }
        }")


type bitmin={n:int;id:int}
let args: string array= fsi.CommandLineArgs
let N=args.[1]|>int 
let Ip = args.[2]
let id = 1 

type taskInfo = {
    actorID :int
    zerofind:int 
}

type env  = {
    env2 : list<string * string>}

let act_num =62

let system = ActorSystem.Create ("System", configuration)


let createRemote (bitmin:bitmin) = 
    let Worker1 =system.ActorSelection("akka.tcp://romoteSystem@%s:9000/user/worker"Ip)
    let taskinfo1 : taskInfo = {
        actorID  = bitmin.id
        zerofind = bitmin.n
    }

    Worker1 <! taskinfo1
    printfn "send request"


type boss (name) = 
    inherit Actor()

    override x.OnReceive(message:obj) = 
        let sender = x.Sender

        match message with
            | :? bitmin as min ->
                   for i =1 to act_num do 
                        let workmessage = {bitmin.n = min.n;bitmin.id=i}
                        createRemote workmessage

            | :?  env  as env ->
                printfn "%A" env.env2
            | :? string ->
                sender <! PoisonPill.Instance

            |_ -> failwith "unkown message" 

let Message = {bitmin.n = N;bitmin.id=id}
let Boss =system.ActorOf(Props(typeof<boss>, [|string("Boss"):> obj|]))

Boss <! Message 


System.Console.ReadLine() 

//Boss <! PoisonPill.Instance

system.Terminate() |> ignore


