﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Model.Filters
{
    public class PageFilter
    {
        private const int DefaultPageIndex = 1;
        private const int DefaultPageSize = 10;

        public PageFilter()
        {
            PageIndex = DefaultPageIndex;
            PageSize = DefaultPageSize;
        }

        private int _index;
        public int PageIndex
        {
            get
            {
                return _index < DefaultPageIndex ? DefaultPageIndex : _index;
            }
            set
            {
                _index = value < DefaultPageIndex ? DefaultPageIndex : value;
            }
        }

        private int _size;
        public int PageSize
        {
            get
            {
                return _size <= 0 ? DefaultPageSize : _size;
            }
            set
            {
                _size = value <= 0 ? DefaultPageSize : _size;
            }
        }

        public int RecordCount { get; set; }
    }
}
