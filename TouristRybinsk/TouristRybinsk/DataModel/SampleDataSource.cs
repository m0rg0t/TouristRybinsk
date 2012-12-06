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

            var group2 = new SampleDataGroup("Group-2",
        "Памятник Бурлаку",
        "",
        "Assets/byrlakryb_thumb.jpg",
        "Памятник Бурлаку");

            group2.Items.Add(new SampleDataItem("Group-2-Item-1",
                    "Памятник Бурлаку",
                    "",
                    "Assets/byrlakryb_thumb.jpg",
                    "Памятник Бурлаку",
                    "Рыбинск когда-то именовался столицей бурлаков. Поэтому не удивительно, что на Волжской набережной в Рыбинске на самом видном месте, недалеко от Рыбинского историко-архитектурного и художественного музея-заповедника, находится памятник Бурлаку. Скульптура установлена на Стоялой улице в 1977 году к 200-летию города.\nПисаревский (автор памятника) мечтал создать для Рыбинска скульптуру могучего труженика. Но не успел осуществить свой замысел.\nПродолжил задуманное друг скульптора М.Е. Удалеев. Он сумел организовать установку модели на Стоялой улице, где некогда толпились тысячи бурлаков, крючников.\nЗдесь скульптура простояла немного лет и ему пришлось уступить место адмиралу Ф.Ф. Ушакову. Бурлака переместили к старой бирже. Интересно узнать, что эта скульптура лишь этюд, который предназначался для музейного зала.\nСкульптура очень сильная, изображает мужественного героя этой тяжелейшей профессии. Сидит усталый бурлак, по-волжски «зимогор», на большом камне и смотрит задумчиво на волжские просторы. Для него река – и кормилица, и нескончаемый труд.\nТрудно теперь представить, что когда-то эти обычные мужики, в большинстве своем крестьяне или бродяги-босяки, буквально на своих плечах тянули груженые баржи! А теперь у скульптуры Бурлака обнимаются влюбленные парочки, играют дети, назначают приятные встречи. Впрочем, так было и в давние времена. Чтобы кому-то веселиться и развлекаться, бурлаки весь сезон тянули свою нелегкую ношу.\nАдрес: Скульптура находится в парковой зоне на Волжской набережной, у старой «хлебной» (лоцманской) биржи  (Волжская наб., д. 4)",
                    group2));

            item1 = new SampleDataItem("Group-2-Item-2",
                    "Фотографии",
                    "",
                    "Assets/byrlakryb_thumb.jpg",
                    "",
                    "",
                    group2);
            item1.SetImage2("Assets/byrlakryb_thumb.jpg");
            group2.Items.Add(item1);

            this.AllGroups.Add(group2);



            var group3 = new SampleDataGroup("Group-3",
            "Старая хлебная биржа",
            "",
            "Assets/birzhastar.jpg",
            "Одним из зданий, которое встречает туристов на берегу Волги в Рыбинске, является старая «лоцманская» биржа.");

            group3.Items.Add(new SampleDataItem("Group-3-Item-1",
                    "Старая хлебная биржа",
                    "",
                    "Assets/birzhastar.jpg",
                    "Старая хлебная биржа",
                    "Одним из зданий, которое встречает туристов на берегу Волги в Рыбинске, является старая «лоцманская» биржа. Строгие пропорции делают это здание одним из лучших памятников провинциального классицизма. Раньше купцы всего Рыбинска торговали здесь хлебом, заключали выгодные сделки. Но так было не всегда.\nСтарая хлебная биржа Рыбинска хранит интересную историю, которая заслуживает освещения на страницах путеводителя.\nАвтором хлебной биржи является архитектор Герасим Варфоломеевич Петров.\nОткрытие состоялось в 1811 году – 18 июля. Кстати средства нашлись за счет благотворительных взносов иногородних купцов, торгующих в Рыбинске (стоимость здания 21437 руб. 58 коп.) На открытии присутствовал сам губернатор Голицин Михаил Николаевич.\nПосле открытия оказалось, что новое здание биржи пустовало, из людей сюда приходили только любопытствующие. И Рыбинская городская дума решила использовать здание под свои нужды. Не занимая большого биржевого зала, она разместила свои городские учреждения там. В 1830-1831 годах после ремонта помещений в биржу заехали уездный и земские суды.\nВ 1841 году проезжая через Рыбинск Николай I дал распоряжение вновь открыть биржу и дать ей надлежащее устройство. Однако и это не убедило рыбинских купцов и все сделки заключались на улицах, в торговых лавках и за чаем.\nВ 1860 году председателем биржи стал М. Н. Журавлев, после чего биржа стала использовать для торговли хлебом и в последующие годы стала крупнейшей в России.\nКак биржа здание существовало до 2 октября 1912 года.  В 1912 стал решать вопрос о строительстве новой биржи, а что же делать со старой? Здание требовало ремонта и шли споры о его дальнейшем назначении. Планировалось даже сдавать его в аренду для торговли. Срочное размещение двух рот Гроховского полка, расквартированного в городе, сняли все вопросы и биржа была занята военными. В советское время здание занимали различные учебные заведения. В послевоенные годы размещались речной вокзал и водная милиция.\nВ последнее время здание старой биржи находится на балансе Рыбинского государственного историко-архитектурного и художественного музея-заповедника. Вскоре здесь планируется открыть экспозицию по истории волжского судоходства и Рыбинской пристани.\n",
                    group2));

            item1 = new SampleDataItem("Group-3-Item-2",
                    "Фотографии",
                    "",
                    "Assets/birzhastar.jpg",
                    "",
                    "",
                    group3);
            item1.SetImage2("Assets/birzhastar.jpg");
            group3.Items.Add(item1);

            item1 = new SampleDataItem("Group-3-Item-3",
                    "Фотографии",
                    "",
                    "Assets/birzharyb.jpg",
                    "",
                    "",
                    group3);
            item1.SetImage2("Assets/birzharyb.jpg");
            group3.Items.Add(item1);

            item1 = new SampleDataItem("Group-3-Item-4",
                "Фотографии",
                "",
                "Assets/vidvolga.jpg",
                "",
                "",
                group3);
            item1.SetImage2("Assets/vidvolga.jpg");
            group3.Items.Add(item1);

            this.AllGroups.Add(group3);






            var group4 = new SampleDataGroup("Group-4",
        "Рыбинское водохранилище",
        "",
        "Assets/rybinskoe-reservior.jpg",
        "Рыбинское водохранилище");

            group4.Items.Add(new SampleDataItem("Group-4-Item-1",
                    "Рыбинское водохранилище",
                    "",
                    "Assets/rybinskoe-reservior.jpg",
                    "Рыбинское водохранилище",
                    "Рыбинское водохранилище – самое большое рукотворное море в Европе.\nПосле его создания Рыбинск получил выход в пять морей. Рыбинское водохранилище привлекает рыбаков и любителей отдыха на воде, позволяет развиваться промышленному рыболовству.  Интересна и ловля рыбы в реке у мест впадения рек в водохранилище.\nМоре в пятнадцать раз больше Московского и превосходит по площади многие водоемы нашей страны.\nНа северо-западном побережье Рыбинского водохранилища расположен Дарвинский заповедник, где водятся медведи, куницы, горностаи, гнездится 230 видов птиц, расположен прекрасный музей природы.\n",
                    group4));

            item1 = new SampleDataItem("Group-4-Item-2",
                    "Фотографии",
                    "",
                    "Assets/rybinskoe-reservior.jpg",
                    "",
                    "",
                    group4);
            item1.SetImage2("Assets/rybinskoe-reservior.jpg");
            group4.Items.Add(item1);

            this.AllGroups.Add(group4);




            var group5 = new SampleDataGroup("Group-5",
        "Рыбинский мост",
        "",
        "Assets/IMG_0257.jpg",
        "Рыбинский мост");

            group5.Items.Add(new SampleDataItem("Group-5-Item-1",
                    "Рыбинский мост",
                    "",
                    "Assets/IMG_0257.jpg",
                    "Рыбинский мост",
                    "Рыбинский мост — уникальная архитектурная постройка и он, на сегодняшний день, является одним  из красивейших мостов на Волге. Мост в в сочетании со Спасо-Преображенским собором без сомнения является самой узнаваемой достопримечательностью Рыбинска.\n\nИстория моста в Рыбинске\n\nЕще в далеком 1938 году сметой Волгостоя предусматривалось строительство Рыбинского моста. Его архитектором является Уланов (родной брат всемирно известной балерины). В 1939 году была установлена часть бетонных опор моста. А в 1941 строительство было остановлено из-за начала Великой Отечественной войны.\nДалее в 1955 году, как сообщает Рыбинский календарь, строительство было возобновлено. К месту строительства были подведены железнодорожный мост, а на берегу Волги построен военный завод. Поставка всех материалов осуществлялась через железнодорожное сообщение Ярославль – Рыбинск, с устройством временного деревянного моста чрез реку Черемуху.\nСтроительство шло успешно, и в 1959 году было закончено строительство 4 опор. Установлено одно пролетное строение, велись работы на подходах к мосту. Представьте, за 3 года был потрачен 21 миллион рублей на строительство. На эти же средства был возведен портальный кран грузоподъемностью в 60 тонн.\nВ августе 1963 года строительство было закончено и 27 августа мост был принят с рядом недоделок, которые устранялись еще 2 года.",
                    group5));

            item1 = new SampleDataItem("Group-4-Item-2",
                    "Фотографии",
                    "",
                    "Assets/IMG_0257.jpg",
                    "",
                    "",
                    group5);
            item1.SetImage2("Assets/IMG_0257.jpg");
            group5.Items.Add(item1);

            item1 = new SampleDataItem("Group-4-Item-3",
                "Фотографии",
                "",
                "Assets/IMG_0262.jpg",
                "",
                "",
                group5);
            item1.SetImage2("Assets/IMG_0262.jpg");
            group5.Items.Add(item1);

            item1 = new SampleDataItem("Group-4-Item-4",
                "Фотографии",
                "",
                "Assets/IMG_0097.jpg",
                "",
                "",
                group5);
            item1.SetImage2("Assets/IMG_0097.jpg");
            group5.Items.Add(item1);

            item1 = new SampleDataItem("Group-4-Item-5",
                "Фотографии",
                "",
                "Assets/IMG_4920.jpg",
                "",
                "",
                group5);
            item1.SetImage2("Assets/IMG_4920.jpg");
            group5.Items.Add(item1);

            this.AllGroups.Add(group5);




            var group6 = new SampleDataGroup("Group-6",
        "Рыбинский музей-заповедник (новая хлебная биржа)",
        "",
        "Assets/IMG_01361.jpg",
        "Рыбинский музей-заповедник (новая хлебная биржа)");

            group6.Items.Add(new SampleDataItem("Group-6-Item-1",
                    "Рыбинский музей-заповедник (новая хлебная биржа)",
                    "",
                    "Assets/IMG_01361.jpg",
                    "Рыбинский музей-заповедник (новая хлебная биржа)",
                    "Рыбинский музей-заповедник (подробнее о музее) — один из лучших на Волге музеев, распологающий свыше 100 тысяч единиц экспонатов.\n\nЗдание новой биржи построено в 1912 году в «неорусском» стиле, использовавшем стилизованные черты древнерусской архитектуры, по проекту архитектора Александра Васильевича Иванова.\nНа момент проектирования биржи Александр Иванов занимал должность архитектора московского Кремля. Строительство новой биржи было продиктовано возросшим авторитетом Рыбинской биржи в торговых кругах России.\nНеоднократно высшие правительственные учреждения запрашивали мнение биржевого комитета по различным экономическим вопросам. Постепенно прежнее здание биржи становится тесным и на берегу Волги появляется величественное здание новой «хлебной» биржи.\nСегодня в здании располагается Рыбинский государственный историко-архитектурный и художественный музей-заповедник.\n",
                    group6));

            item1 = new SampleDataItem("Group-6-Item-2",
                    "Фотографии",
                    "",
                    "Assets/IMG_01361.jpg",
                    "",
                    "",
                    group6);
            item1.SetImage2("Assets/IMG_01361.jpg");
            group6.Items.Add(item1);

            this.AllGroups.Add(group6);




            var group7 = new SampleDataGroup("Group-7",
                "Дом художников",
                "",
                "Assets/dom-hydoznikov.jpg",
                "Дом художников");

            group7.Items.Add(new SampleDataItem("Group-7-Item-1",
                    "Дом художников",
                    "",
                    "Assets/dom-hydoznikov.jpg",
                    "Дом художников",
                    "Прекрасным образцом деревянной архитектуры начала ХХ века является двухэтажное здание, расположенное на углу улиц Пушкина и Плеханова (ул. Пушкина, №52), фотографии которого украшают современные буклеты и проспекты, рассказывающие об историческом Рыбинске.\nВ 1900 году С.Г. Гордеев выстроил этот наугольный дом, который двумя фасадами выходил на обе улицы. Находившийся поблизости железнодорожный вокзал придавал особую значимость этому бойкому перекрестку, и домовладелец, конечно, постарался выстроить здесь такую «хоромину», которую не стыдно было выставить напоказ на людном месте.\nНад угловой частью обшитого тесом особняка возвышались два высоких шатра, каждый из которых был ориентирован на свою улицу и имел очень сложное завершение в виде крещатой бочки  с коваными навершиями. Особый колорит перекрестку улиц придавал внушительный балкон второго этажа с кованой ажурной решеткой, полукругом объединявший фасады дома. Украшением дома были и наличники окон, условно повторяющих формы нарышкинского барокко.\nНедавно этот уникальный дом был реконструирован, с сохранением деревянной обшивки и декорации.\n",
                    group7));

            item1 = new SampleDataItem("Group-7-Item-2",
                    "Фотографии",
                    "",
                    "Assets/dom-hydoznikov.jpg",
                    "",
                    "",
                    group7);
            item1.SetImage2("Assets/dom-hydoznikov.jpg");
            group7.Items.Add(item1);

            this.AllGroups.Add(group7);





            var group8 = new SampleDataGroup("Group-8",
                "Никольская часовня",
                "",
                "Assets/nicolsk.jpg",
                "Никольская часовня");

            group8.Items.Add(new SampleDataItem("Group-8-Item-1",
                    "Никольская часовня",
                    "",
                    "Assets/nicolsk.jpg",
                    "Никольская часовня",
                    "Раньше, спускаясь по Стоялой улице в Рыбинске, случайных прохожий вряд ли обратил бы внимание на Никольскую часовню. Сегодня это двухэтажное незаметное сооружение с неказистым видом сегодня приняло новый облик. На фотографии выше вы можете видеть здание еще в процессе реставрации.\nУже сейчас на главе часовни установлен крест. Кроме главного креста 10 сентября установлены шесть небольших крестов на главках, украшающих купол часовни. Позднее появятся еще шесть. Все кресты отлиты фирмой «Ярославский реставратор» по правилам, действующим в 19 веке, и покрыты сусальным золотом.\nКак сообщает официальный сайт города, полная реставрация закончатся к маю 2011 года.\n",
                    group8));

            item1 = new SampleDataItem("Group-8-Item-2",
                    "Фотографии",
                    "",
                    "Assets/nicolsk.jpg",
                    "",
                    "",
                    group7);
            item1.SetImage2("Assets/nicolsk.jpg");
            group8.Items.Add(item1);

            item1 = new SampleDataItem("Group-8-Item-3",
                    "Фотографии",
                    "",
                    "Assets/Nicchas.jpg",
                    "",
                    "",
                    group7);
            item1.SetImage2("Assets/Nicchas.jpg");
            group8.Items.Add(item1);


            this.AllGroups.Add(group8);



        }
    }
}
