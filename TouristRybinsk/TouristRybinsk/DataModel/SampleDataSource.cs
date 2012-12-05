using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.Specialized;

// Модель данных, определяемая этим файлом, служит типичным примером строго типизированной
// модели, которая поддерживает уведомление при добавлении, удалении или изменении членов. Выбранные
// имена свойств совпадают с привязками данных из стандартных шаблонов элементов.
//
// Приложения могут использовать эту модель в качестве начальной точки и добавлять к ней дополнительные элементы или полностью удалить и
// заменить ее другой моделью, соответствующей их потребностям.

namespace TouristRybinsk.Data
{
    /// <summary>
    /// Базовый класс объектов <see cref="SampleDataItem"/> и <see cref="SampleDataGroup"/>, который
    /// определяет свойства, общие для обоих объектов.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : TouristRybinsk.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }
        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }



        private ImageSource _image2 = null;
        private String _imagePath2 = null;
        public ImageSource Image2
        {
            get
            {
                if (this._image2 == null && this._imagePath2 != null)
                {
                    this._image2 = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imagePath2));
                }
                return this._image2;
            }

            set
            {
                this._imagePath2 = null;
                this.SetProperty(ref this._image2, value);
            }
        }
        public void SetImage2(String path)
        {
            this._image2 = null;
            this._imagePath2 = path;
            this.OnPropertyChanged("Image2");
        }



        public override string ToString()
        {
            return this.Title;
        }
    }

    /// <summary>
    /// Универсальная модель данных элементов.
    /// </summary>
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, SampleDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private SampleDataGroup _group;
        public SampleDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
    }

    /// <summary>
    /// Универсальная модель данных групп.
    /// </summary>
    public class SampleDataGroup : SampleDataCommon
    {
        public SampleDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            Items.CollectionChanged += ItemsCollectionChanged;
        }

        private void ItemsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Предоставляет подмножество полной коллекции элементов, привязываемой из объекта GroupedItemsPage
            // по двум причинам: GridView не виртуализирует большие коллекции элементов и оно
            // улучшает работу пользователей при просмотре групп с большим количеством
            // элементов.
            //
            // Отображается максимальное число столбцов (12), поскольку это приводит к заполнению столбцов сетки
            // сколько строк отображается: 1, 2, 3, 4 или 6

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex < 12)
                    {
                        TopItems.Insert(e.NewStartingIndex,Items[e.NewStartingIndex]);
                        if (TopItems.Count > 12)
                        {
                            TopItems.RemoveAt(12);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (e.OldStartingIndex < 12 && e.NewStartingIndex < 12)
                    {
                        TopItems.Move(e.OldStartingIndex, e.NewStartingIndex);
                    }
                    else if (e.OldStartingIndex < 12)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        TopItems.Add(Items[11]);
                    }
                    else if (e.NewStartingIndex < 12)
                    {
                        TopItems.Insert(e.NewStartingIndex, Items[e.NewStartingIndex]);
                        TopItems.RemoveAt(12);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex < 12)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        if (Items.Count >= 12)
                        {
                            TopItems.Add(Items[11]);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldStartingIndex < 12)
                    {
                        TopItems[e.OldStartingIndex] = Items[e.OldStartingIndex];
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    TopItems.Clear();
                    while (TopItems.Count < Items.Count && TopItems.Count < 12)
                    {
                        TopItems.Add(Items[TopItems.Count]);
                    }
                    break;
            }
        }

        private ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> Items
        {
            get { return this._items; }
        }

        private ObservableCollection<SampleDataItem> _topItem = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> TopItems
        {
            get {return this._topItem; }
        }
    }

    /// <summary>
    /// Создает коллекцию групп и элементов с жестко заданным содержимым.
    /// 
    /// SampleDataSource инициализируется подстановочными данными, а не реальными рабочими
    /// данными, чтобы пример данных был доступен как во время разработки, так и во время выполнения.
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();
        public ObservableCollection<SampleDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            
            return _sampleDataSource.AllGroups;
        }

        public static SampleDataGroup GetGroup(string uniqueId)
        {
            // Для небольших наборов данных можно использовать простой линейный поиск
            var matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // Для небольших наборов данных можно использовать простой линейный поиск
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public SampleDataSource()
        {
            String ITEM_CONTENT = String.Format("isque feugiat");

            var group1 = new SampleDataGroup("Group-1",
                    "Спасо-Преображенский собор",
                    "",
                    "Assets/sobor-volga.jpg",
                    "Спасо-Преображенский собор – жемчужина исторического центра города Рыбинска.");
            
            group1.Items.Add(new SampleDataItem("Group-1-Item-1",
                    "Спасо-Преображенский собор - описание",
                    "",
                    "Assets/sobor-volga.jpg",
                    "Спасо-Преображенский собор – жемчужина исторического центра города Рыбинска.",
                    "Спасо-Преображенский собор – жемчужина исторического центра города Рыбинска.\nПрородителем Спасо-Преображенского собора была деревянная церковь в честь апостола Петра, покровителя рыбаков. В XVII веке на месте обветшалой деревянной построили каменную, во  имя Преображения Господня, которая стала соборной в 1778 году, затем ее перестроили и в 1804 году рядом с ней возвели новую, каменную колокольню. Взору горожан предстала устремленная в небо почти стометровая свеча, украшенная колоннами.\nГород рос, росло и количество православных жителей. Вопрос о строительстве в Рыбинске более вместительного собора возник еще в самом начале XIX века, но по разным причинам откладывался, и решение затянулось на двадцать пять лет. В 1838 году старый собор все-таки разбирают и начинают возводить новый, который потом назовут «красой Поволжья».\nОказывается, что проект собора, прежде чем оказался в Рыбинске, участвовал в конкурсе на строительство Исаакиевского собора в Петербурге и занял там третье место. В 18831-1851 гг. по проекту группы петербургских архитекторов во главе с А. И. Мельниковым на месте одноимённой церкви XVII века Петра и Павла был воздвигнут Спасо-Преображенский собор. Работы по строительству шли тринадцать лет, но результат превзошел все ожидания. Огромное здание в форме куба венчали мощные купола, опирающиеся на массивные барабаны, северный и южный фасады украшали высокие колонны.\nВнутреннее убранство собора также ошеломляло своей идее. Пол выложен гранитными плитами, стены отделаны под белый мрамор. Для оформления интерьера израсходовали более пятисот килограммов позолоченного серебра. Мастер из Ростова Великого В.М. Бычков сделал для собора огромный четырехъярусный иконостас, в котором были древние иконы XV века. В центре собора под бархатным балдахином поставили главную реликвию города — нарядное кресло, сделанное специально к приезду Екатерины II.\nСегодня Спасо-Преображенский собор воспринимается как единый архитектурный ансамбль, созданный по единому проекту. Однако пятиярусная колокольня построена на 50 лет раньше самого храма, предположительно по проекту костромского зодчего-самоучки Степана Воротилова. Её архитектуре присуща и мягкая живописность барокко, и изящная сдержанность классицизма, в отличие от величавой архитектуры Собора.",
                    group1));
            SampleDataItem item1 = new SampleDataItem("Group-1-Item-2",
                    "Фотографии",
                    "",
                    "Assets/sobor-volga2.jpg",
                    "",
                    "",
                    group1);
            item1.SetImage2("Assets/sobor-volga2.jpg");
            group1.Items.Add(item1);

            item1 = new SampleDataItem("Group-1-Item-3",
            "Фотографии",
            "",
            "Assets/sobor-volga-most.jpg",
            "",
            "",
            group1);
            item1.SetImage2("Assets/sobor-volga-most.jpg");
            group1.Items.Add(item1);

            item1 = new SampleDataItem("Group-1-Item-4",
            "Фотографии",
            "",
            "Assets/sobor-ryb.jpg",
            "",
            "",
            group1);
            item1.SetImage2("Assets/sobor-ryb.jpg");
            group1.Items.Add(item1);

            item1 = new SampleDataItem("Group-1-Item-5",
            "Фотографии",
            "",
            "Assets/rybinsk-teplohod.jpg",
            "",
            "",
            group1);
            item1.SetImage2("Assets/rybinsk-teplohod.jpg");
            group1.Items.Add(item1);

            item1 = new SampleDataItem("Group-1-Item-8",
            "Фотографии",
            "",
            "Assets/rybinsk-sobor.jpg",
            "",
            "",
            group1);
            item1.SetImage2("Assets/rybinsk-sobor.jpg");
            group1.Items.Add(item1);

            this.AllGroups.Add(group1);
        }
    }
}
