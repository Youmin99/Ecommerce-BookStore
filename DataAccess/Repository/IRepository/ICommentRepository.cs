﻿using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Repository;
using DataAccess.Data;

namespace DataAccess.Repository.IRepository
{
    public interface ICommentRepository : IRepository<Comment>
    {
        void Update(Comment obj);

	}
}
