﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Fcog.Core.Localization {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class CoreUI {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CoreUI() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Fcog.Core.Localization.CoreUI", typeof(CoreUI).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Count.
        /// </summary>
        public static string CharacterCountSign {
            get {
                return ResourceManager.GetString("CharacterCountSign", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Frequency.
        /// </summary>
        public static string CharacterFrequencySign {
            get {
                return ResourceManager.GetString("CharacterFrequencySign", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Index.
        /// </summary>
        public static string CharacterIndexSign {
            get {
                return ResourceManager.GetString("CharacterIndexSign", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Value.
        /// </summary>
        public static string CharacterValueSign {
            get {
                return ResourceManager.GetString("CharacterValueSign", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Check marks.
        /// </summary>
        public static string CheckMarkMachineSign {
            get {
                return ResourceManager.GetString("CheckMarkMachineSign", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Create Form Mode.
        /// </summary>
        public static string CreateModeSign {
            get {
                return ResourceManager.GetString("CreateModeSign", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Digits.
        /// </summary>
        public static string DigitsMachineSign {
            get {
                return ResourceManager.GetString("DigitsMachineSign", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Mark Question.
        /// </summary>
        public static string QuestionTypeMarkSign {
            get {
                return ResourceManager.GetString("QuestionTypeMarkSign", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Multi Answers Question.
        /// </summary>
        public static string QuestionTypeMultiSign {
            get {
                return ResourceManager.GetString("QuestionTypeMultiSign", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Recognition Text Question.
        /// </summary>
        public static string QuestionTypeRecogTextSign {
            get {
                return ResourceManager.GetString("QuestionTypeRecogTextSign", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Single Answer Question.
        /// </summary>
        public static string QuestionTypeSingleSign {
            get {
                return ResourceManager.GetString("QuestionTypeSingleSign", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Simple Handwritten Text.
        /// </summary>
        public static string QuestionTypeTextSign {
            get {
                return ResourceManager.GetString("QuestionTypeTextSign", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Questionnaire are not valid. My be questions count in one of the form is 0 or cell dostances from marker are null.
        /// </summary>
        public static string ThrowQuestionnaireNotValid {
            get {
                return ResourceManager.GetString("ThrowQuestionnaireNotValid", resourceCulture);
            }
        }
    }
}
