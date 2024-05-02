using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharedKernel.Runtime.Exceptions;

namespace SharedKernel.Application
{
    public class PagingRequest
    {
        private int _page = 0;
        private int _size = 20;
        private int _indexFrom = 1;

        public int Page
        {
            get
            {
                return _page;
            }

            set
            {
                if (value < 0)
                {
                    throw new BadRequestException("Page must be greater than or equal 0");
                }
                _page = value;
            }
        }

        public int Size
        {
            get
            {
                return _size;
            }

            set
            {
                if (value <= 0 || value > 1000)
                {
                    throw new BadRequestException("Size should be between 1 and 1000");
                }
                _size = value;
            }
        }
        
        public int IndexFrom
        {
            get
            {
                return _indexFrom;
            }

            set
            {
                if (value < 0 || value > 1000)
                {
                    throw new BadRequestException("Size should be between 1 and 1000");
                }
                _indexFrom = value;
            }
        }

        public int Offset => (_page - _indexFrom) * _size;

        public Filter Filter { get; set; } = new Filter();

        public string SearchString { get; set; } = string.Empty;
        public List<SortModel> Sorts { get; set; } = new List<SortModel>();

        public PagingRequest(int page, int size)
        {
            Page = page;
            Size = size;
        }

        public PagingRequest()
        {
            
        }
    }
}
