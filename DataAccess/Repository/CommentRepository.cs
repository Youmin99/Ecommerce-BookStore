using DataAccess.Repository;
using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        private ApplicationDbContext _db;

        public CommentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(Comment obj)
        {
            _db.Comments.Update(obj);
        }
    }
}
