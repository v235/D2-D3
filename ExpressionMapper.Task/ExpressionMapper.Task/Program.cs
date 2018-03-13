using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionMapper.Task
{
    class Program
    {
        static void Main(string[] args)
        {
            ModelDAO mDAO = new ModelDAO()
            {
                Id = 1,
                Name = "test",
                Date = DateTime.Now,
                Rule = false,
                SecondName = "test2"
            };
            var mappermDAO = MappingGenerator.Generate<ModelDAO, ModelDTO>();
            var resDTO = mappermDAO.Map(mDAO);
            Console.WriteLine("{0}|{1}", mDAO.Id, resDTO.Id);
            Console.WriteLine("{0}|{1}", mDAO.Name, resDTO.Name);
            Console.WriteLine("{0}|{1}", mDAO.Date, resDTO.Date);
            Console.WriteLine("{0}|{1}", mDAO.Rule, resDTO.Rule);
            Console.WriteLine("{0}|", mDAO.SecondName);

            ModelDTO mDTO = new ModelDTO()
            {
                Id = 5,
                Name = "test7",
                Date = DateTime.Now,
                Rule = true,
            };
            var mappermDTO = MappingGenerator.Generate<ModelDTO, ModelDAO>();
            var resDAO = mappermDTO.Map(mDTO);
            Console.WriteLine("{0}|{1}", mDTO.Id, resDAO.Id);
            Console.WriteLine("{0}|{1}", mDTO.Name, resDAO.Name);
            Console.WriteLine("{0}|{1}", mDTO.Date, resDAO.Date);
            Console.WriteLine("{0}|{1}", mDTO.Rule, resDAO.Rule);
            Console.WriteLine("|{0}", resDAO.SecondName);

            Console.ReadKey();
        }
    }
}
