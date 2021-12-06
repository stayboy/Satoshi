namespace Satoshi.Enums;
   
[Flags]
public enum OrderSort
{
    None = 0,
    Customer_Name = 1,
    Product_Name = 2,
    Price = 4,
    Price_Desc = 8,
    Product_Name_Desc = 16,
    Customer_Name_Desc = 32
}

[Flags]
public enum ProductSort
{
    None = 0,
    Product_Name = 1,
    Price = 2,
    ProductName_Desc = 4,
    Price_Desc = 8
}
