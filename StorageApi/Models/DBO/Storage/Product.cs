﻿using System.ComponentModel.DataAnnotations;

namespace StorageApi.Models.DBO.Storage
{
    public class Product
    {
        [Key]
        public long Id { get; set; }
        public Brand Brand { get; set; }
        public string Name { get; set; }
    }
}