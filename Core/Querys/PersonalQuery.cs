﻿namespace Core.Querys;

public class PersonalQuery
{
    public string search { get; set; } 
    public string gender { get; set; }
    public string branch { get; set; } 
    public string position { get; set; } 
    public string retired { get; set; } 
    public int sayfa { get; set; } = 1;
    public string sortName { get; set; }
    public string sortBy { get; set; }
    
}