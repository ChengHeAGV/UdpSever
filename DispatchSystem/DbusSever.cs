//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DispatchSystem
{
    using System;
    using System.Collections.Generic;
    
    public partial class DbusSever
    {
        public int Id { get; set; }
        public string key { get; set; }
        public int value { get; set; }
        public string des { get; set; }
        public override string ToString()
        {
            //String.Format("{0,-10}", str);//这个表示第一个参数str字符串的宽度为10，左对齐
            //String.Format("{0,10}", str);//这个表示第一个参数str字符串的宽度为10，右对齐
            return string.Format("ID: {0,-5}Key: {1,-20}Value: {2,-10}Des: {3,-20}", Id,key,value,des);
        }
    }
}
