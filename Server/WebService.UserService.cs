using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;

namespace Server
{
    public partial class WebService
    {

        /// <summary>
        /// 查找活动和奖品情况
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public bool Update_User()
        {
            //if (!unid.IsNotNullOrEmpty())
            //    return null;
            //using (DbRepository entities = new DbRepository())
            //{
            //    Domain.ScratchCard.Update model = new Update();
            //    var scratchScardEntity = entities.ScratchCard.Find(unid);
            //    var prizeEntity = entities.Prize.Where(x => x.TargetCode == (int)TargetCode.ScratchCard && x.TargetID.Equals(unid)).FirstOrDefault();
            //    if (prizeEntity != null)
            //    {
            //        prizeEntity.AutoMap<Prize, Domain.ScratchCard.Update>(model);
            //    }
            //    if (scratchScardEntity != null)
            //        scratchScardEntity.AutoMap<ScratchCard, Domain.ScratchCard.Update>(model);


            //    return model;
            //}
            return true;
        }
    }
}
