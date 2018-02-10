module MailService

open System.Diagnostics

type IMailService = 
    abstract member sendMail: string * string -> unit



type LocalMailService() =
    interface IMailService with
        member this.sendMail (subject: string, message: string) = 
            do Debug.WriteLine (sprintf "Mail from %s sent to %s, with LocalMailService." this.mailFrom this.mailTo)
               Debug.WriteLine (sprintf "Subject: %s" subject)
               Debug.WriteLine (sprintf "Message: %s" message)
            

    member private this.mailTo = "email@user.com"
    member private this.mailFrom = "email@domain.com"





type CloudMailService() =
    interface IMailService with
        member this.sendMail (subject: string, message: string) = 
            do Debug.WriteLine (sprintf "Mail from %s sent to %s, with LocalMailService." this.mailFrom this.mailTo)
               Debug.WriteLine (sprintf "Subject: %s" subject)
               Debug.WriteLine (sprintf "Message: %s" message)
            

    member private this.mailTo = "someMail@yahoo.com"
    member private this.mailFrom = "fromMail@yahoo.com"
