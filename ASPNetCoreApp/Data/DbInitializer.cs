using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNetCoreApp.Models;


namespace ASPNetCoreApp.Data
{
	public static class DbInitializer
	{
		public static void Initialize(fnewsContext context)
		{
			context.Database.EnsureCreated();
			if (context.Topic.Any())
			{
				return;
			}
			var topics = new Topic[]
			{
				new Topic {Url= "http://topics.msdn.com/rfpl", Name="Российская премьер-лига" },
				new Topic {Url= "http://topics.msdn.com/epl", Name="Английская премьер-лига"  },
				new Topic {Url= "http://topics.msdn.com/seriaa", Name="Сериа А"  }
			};
			foreach (Topic t in topics)
			{
				context.Topic.Add(t);
			}
			context.SaveChanges();

			var novelties = new Novelty[]
			{
				new Novelty { TopicId= 1 , Content= "Нападающий «Зенита» Александру Кокорину рассказал, как отметит день рождения в СИЗО.Во вторник форварду сборной России исполняется 28 лет. «Я думаю, что воспользуюсь разрешением следователя на звонок семье. Никаких подарков ни от кого я не просил — ни от родных, ни от друзей»" ,Title= "Кокорин о дне рождения в СИЗО" },
				new Novelty { TopicId= 1 , Content= "РПЛ назвала лучшего игрока 20-го тура чемпионата России.Им стал защитник ЦСКА Марио Фернандес. В голосовании болельщиков он набрал 43% (чуть больше 27 тысяч голосов) и опередил полузащитника «Зенита» Вильмара Барриоса." ,Title= "Фернандес – лучший игрок 20-го тура РПЛ" }
			};

			foreach (Novelty n in novelties)
			{
				context.Novelty.Add(n);
			}
			context.SaveChanges();
		}
	}
}



