
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
    
public partial class Tournament
{

    public int Id { get; set; }

    public string date { get; set; }

    public string organizer { get; set; }

    public int prize_fund { get; set; }

    public string status { get; set; }

    public int id_game { get; set; }



    public virtual Game Game { get; set; }

}

}
