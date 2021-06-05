// <copyright file="ListModel.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.Domains.Models
{
    using System.Collections.Generic;

    public class ListModel<T>
        where T : class
    {
        public int RecordsQuantity { get; set; }

        public IEnumerable<T> Records { get; set; }
    }
}
