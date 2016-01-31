using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechStackDemo.Repository.Counter
{
    public class CounterItem
    {
        public CounterItem(string id, int value)
        {
            this.Id = id;
            this.Value = value;
        }

        public string Id { get; }
        public int Value { get; }

        public CounterItem WithValue(int value)
        {
            return new CounterItem(Id, value);
        }

        public CounterItemDto ToDto()
        {
            return new CounterItemDto(this);
        }
    }

    public class CounterItemDto
    {
        public CounterItemDto(CounterItem item)
        {
            this.Id = item.Id;
            this.Value = item.Value;
        }

        public CounterItemDto(string id)
        {
            this.Id = id;
            this.Value = null;
        }

        public string Id { get; }
        public int? Value { get; }
    }
}