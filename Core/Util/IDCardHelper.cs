using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Extensions;

namespace Core.Util
{
    /// <summary>
    /// 身份证
    /// </summary>
    public class IDCardHelper
    {
        //如果是15位身份证号码，则自动转换为18位
        public IDCardHelper(String cardNumber)
        {
            if (null != cardNumber)
            {
                cardNumber = cardNumber.Trim();
                if (OLD_CARD_NUMBER_LENGTH == cardNumber.Length)
                {
                    cardNumber = ContertToNewCardNumber(cardNumber);
                }
            }
            this.cardNumber = cardNumber;
        }

        private String cardNumber; // 完整的身份证号码
        private bool cacheValidateResult = true;    // 缓存身份证是否有效，因为验证有效性使用频繁且计算复杂
        private DateTime cacheBirthDate;    // 缓存出生日期，因为出生日期使用频繁且计算复杂

        private const String BIRTH_DATE_FORMAT = "yyyyMMdd";  // 身份证号码中的出生日期的格式
        private DateTime MINIMAL_BIRTH_DATE = new DateTime(1900, 1, 1);  // 身份证的最小出生日期,1900年1月1日
        private const int NEW_CARD_NUMBER_LENGTH = 18;
        private const int OLD_CARD_NUMBER_LENGTH = 15;
        private static char[] VERIFY_CODE = { '1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2' }; //18位身份证中最后一位校验码
        private static int[] VERIFY_CODE_WEIGHT = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 }; //18位身份证中，各个数字的生成校验码时的权值

        #region 私有成员
        /// <summary>
        /// 获取身份证的第17位，奇数为男性，偶数为女性
        /// </summary>
        /// <returns></returns>
        private int GetGenderCode()
        {
            this.CheckIfValid();
            char genderCode = this.cardNumber[NEW_CARD_NUMBER_LENGTH - 2];
            return (((int)(genderCode - '0')) & 0x1);
        }
        private String GetBirthDayPart()
        {
            return this.cardNumber.Substring(6, 8);
        }
        private void CheckIfValid()
        {
            if (false == this.Validate())
            {
                throw new Exception("身份证号码不正确！");
            }
        }
        /// <summary>
        /// 校验码（第十八位数）：
        /// 十七位数字本体码加权求和公式 S = Sum(Ai * Wi), i = 0...16 ，先对前17位数字的权求和；
        /// Ai:表示第i位置上的身份证号码数字值 Wi:表示第i位置上的加权因子 Wi: 7 9 10 5 8 4 2 1 6 3 7 9 10 5 8 4 2；
        /// 计算模 Y = mod(S, 11)
        /// 通过模得到对应的校验码 Y: 0 1 2 3 4 5 6 7 8 9 10 校验码: 1 0 X 9 8 7 6 5 4 3 2
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        private static char CalculateVerifyCode(string cardNumber)
        {
            int sum = 0;
            for (int i = 0; i < NEW_CARD_NUMBER_LENGTH - 1; i++)
            {
                char ch = cardNumber[i];
                sum += ((int)(ch - '0')) * VERIFY_CODE_WEIGHT[i];
            }
            return VERIFY_CODE[sum % 11];
        }
        /// <summary>
        /// 把15位身份证号码转换到18位身份证号码          
        /// 15位身份证号码与18位身份证号码的区别为：          
        /// 1、15位身份证号码中，"出生年份"字段是2位，转换时需要补入"19"，表示20世纪          
        /// 2、15位身份证无最后一位校验码。18位身份证中，校验码根据根据前17位生成
        /// </summary>
        /// <param name="oldCardNumber"></param>
        /// <returns></returns>
        private static String ContertToNewCardNumber(String oldCardNumber)
        {
            StringBuilder buf = new StringBuilder(NEW_CARD_NUMBER_LENGTH);
            buf.Append(oldCardNumber.Substring(0, 6));
            buf.Append("19");
            buf.Append(oldCardNumber.Substring(6));
            buf.Append(IDCardHelper.CalculateVerifyCode(buf.ToString()));
            return buf.ToString();

        }
        #endregion 私有成员

        public bool Validate()
        {
            bool result = true;
            // 身份证号不能为空
            result = result && (null != cardNumber);
            // 身份证号长度是18(新证)
            result = result && NEW_CARD_NUMBER_LENGTH == cardNumber.Length;
            // 身份证号的前17位必须是阿拉伯数字
            for (int i = 0; result && i < NEW_CARD_NUMBER_LENGTH - 1; i++)
            {
                char ch = cardNumber[i];
                result = result && ch >= '0' && ch <= '9';
            }
            // 身份证号的第18位校验正确
            result = result && (CalculateVerifyCode(cardNumber) == cardNumber[NEW_CARD_NUMBER_LENGTH - 1]);
            // 出生日期不能晚于当前时间，并且不能早于1900年
            try
            {
                DateTime birthDate = this.GetBirthDate();
                result = result && null != birthDate;
                result = result && (birthDate > new DateTime());
                result = result && (birthDate > MINIMAL_BIRTH_DATE);
                /**
                 * 出生日期中的年、月、日必须正确,比如月份范围是[1,12],日期范围是[1,31]，还需要校验闰年、大月、小月的情况时，
                 * 月份和日期相符合
                 */
                String birthdayPart = this.GetBirthDayPart();
                String realBirthdayPart = birthDate.ToString(BIRTH_DATE_FORMAT);
                result = result && (birthdayPart.Equals(realBirthdayPart));
            }
            catch (Exception e)
            {
                result = false;
            }
            // TODO 完整身份证号码的省市县区检验规则
            cacheValidateResult = result;
            return cacheValidateResult;
        }
        public String GetCardNumber()
        {
            return cardNumber;
        }
        public String GetAddressCode()
        {
            this.CheckIfValid();
            return this.cardNumber.Substring(0, 6);
        }
        public DateTime GetBirthDate()
        {
            try
            {
                string birthDaypart = this.GetBirthDayPart();
                this.cacheBirthDate = new DateTime(birthDaypart.Substring(0, 4).GetInt(), birthDaypart.Substring(4, 2).GetInt(), birthDaypart.Substring(6, 2).GetInt());
            }
            catch (Exception e)
            {
                throw new Exception("身份证的出生日期无效");
            }
            return this.cacheBirthDate;
        }
        public bool IsMale()
        {
            return 1 == this.GetGenderCode();
        }
        public bool IsFemal()
        {
            return false == this.IsMale();
        }
    }
}
