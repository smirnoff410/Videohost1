
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Videohost.Models
{

using System;
    using System.Collections.Generic;
    
public partial class Message
{

    public int Id { get; set; }

    public int id_user { get; set; }

    public string text { get; set; }

    public string date { get; set; }



    public virtual User User { get; set; }

}

}
