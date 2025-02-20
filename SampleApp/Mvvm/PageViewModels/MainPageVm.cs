﻿using SampleApp.Mvvm.ViewModels;
using SampleApp.Mvvm.ViewModels.TemplateTest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp.Mvvm.PageViewModels
{
    public class MainPageVm : BaseVm
    {
        private TestNode _spareNode;
        private LevelTwo _spareTemplateNode;

        private bool _treeDance;
        public bool TreeDance
        {
            get => _treeDance;
            set => SetProperty(ref _treeDance, value);
        }

        private bool _listDance;
        public bool ListDance
        {
            get => _listDance;
            set => SetProperty(ref _listDance, value);
        }

        private ListItem _selectedItem;
        public ListItem SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        private ObservableCollection<ListItem> _selectedItems;
        public ObservableCollection<ListItem> SelectedItems
        {
            get => _selectedItems;
            set => SetProperty(ref _selectedItems, value);
        }

        private float _listViewScrollOffset;
        public float ListViewScrollOffset
        {
            get => _listViewScrollOffset;
            set => SetProperty(ref _listViewScrollOffset, value);
        }

        private float _treeViewScrollOffset;
        public float TreeViewScrollOffset
        {
            get => _treeViewScrollOffset;
            set => SetProperty(ref _treeViewScrollOffset, value);
        }

        public class PickerStuff
        {
            public PickerStuff(string name, SelectionMode mode)
            {
                Name = name;
                Mode = mode;
            }

            public string Name { get; }
            public SelectionMode Mode { get; }
        }

        public List<PickerStuff> PickerData { get; }

        public class ListItem : INotifyPropertyChanged
        {
            private string _name;
            private float _offset;

            public ListItem(string name, float offset)
            {
                Name = name;
                Offset = offset;
            }

            public string Name
            {
                get => _name;
                set
                {
                    if (_name != value)
                    {
                        _name = value;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                    }
                }
            }
            public float Offset
            {
                get => _offset;
                set
                {
                    if (_offset != value)
                    {
                        _offset = value;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Offset)));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }
        public MainPageVm()
        {
            SampleData = GetSampleTree();

            SampleListData = new ObservableCollection<ListItem>();

            for (int c = 0; c < 400; c++)
                SampleListData.Add(new ListItem($"Hello {c}", (float)110.0 + (float)Math.Sin(c / 9.0) * 40));

            SelectedItems = new ObservableCollection<ListItem>();
            SelectedItems.CollectionChanged += (sender, e) => { Debug.WriteLine($"VM Count:{SelectedItems.Count}"); };

            PickerData = new()
            {
                new PickerStuff("None", SelectionMode.None),
                new PickerStuff("Single", SelectionMode.Single),
                new PickerStuff("Multiple", SelectionMode.Multiple),
            };

            SampleTemplateTestData = new LevelZero("Root") { IsLevelZeroExpanded = true };

            Device.StartTimer(TimeSpan.FromMilliseconds(77), Tick);
            //Device.StartTimer(TimeSpan.FromMilliseconds(15), Tick);

            Device.StartTimer(TimeSpan.FromMilliseconds(20), Tick2);


            Task.Delay(1500).ContinueWith((d) => SelectedItem = (ListItem)SampleListData[4]);
            Task.Delay(2000).ContinueWith((d) => SelectedItem = (ListItem)SampleListData[5]);
            Task.Delay(2500).ContinueWith((d) => SelectedItem = (ListItem)SampleListData[6]);

        }

        private int _listCount;
        private bool Tick2()
        {
            if (ListDance == false)
                return true;
#if false


            if (_listCount % 2 == 0)
                for (int c = 1; c < 6; c++)
                    SampleListData.RemoveAt(0);
            else
                for (int c = 1; c < 6; c++)
                    SampleListData.Insert(c, new ListItem($"BORG {_listCount}", (float)110.0 + (float)Math.Sin(_listCount / 9.0) * 40));

            ((ListItem)SampleListData[0]).Name = _listCount.ToString();
            ((ListItem)SampleListData[0]).Offset = (float)110.0 + (float)Math.Sin(_listCount / 9.0) * 40;


            return true;

#elif false

            //var scale = (Math.Sin(_listCount / 223.0 * Math.Cos(_listCount / 337.0))) / 2.0 + 1.0;
            var scale = Math.Sin(_listCount / 223.0) / 2.0 + 1.0;
            ListViewScrollOffset = (float)scale * SampleListData.Count * 25;

#elif true

            //if ((_listCount % 16) == 0)
                for (int c = 0; c < 8; c++)
                {
                    //if (((_listCount>>4) & (1 << c)) != 0)
                    if (((_listCount) & (1 << c)) != 0)
                    {
                        if (SelectedItems.Contains(SampleListData[c + 10]) == false)
                            SelectedItems.Add((ListItem)SampleListData[c + 10]);
                    }
                    else
                    {
                        if (SelectedItems.Contains(SampleListData[c + 10]) == true)
                            SelectedItems.Remove((ListItem)SampleListData[c + 10]);
                    }
                }
#endif
            _listCount++;
            return true;
        }

        private int _count;
        private bool _isRootVisible;

        public int Count { get => _count; set => SetProperty(ref _count, value); }
        public bool IsRootVisible { get => _isRootVisible; set => SetProperty(ref _isRootVisible, value); }

        private bool Tick()
        {
            if (TreeDance == false)
                return true;

            //var scale = (Math.Sin(_listCount / 223.0 * Math.Cos(_listCount / 337.0))) / 2.0 + 1.0;
            var scale = Math.Sin(Count / 2.0) / 2.0;
            TreeViewScrollOffset = (float)scale * 100;
            Count++;
            return true;
            //IsRootVisible = (Count & 8)==0;
            if ((Count % 8) < 2)
            {
                ((TestNode)SampleData).IsDataExpanded = !((TestNode)SampleData).IsDataExpanded;
                SampleTemplateTestData.IsLevelZeroExpanded = !SampleTemplateTestData.IsLevelZeroExpanded;
            }

            if ((Count % 3) != 0)
            {
                ((TestNode)SampleData).Children[1].IsDataExpanded = !((TestNode)SampleData).Children[1].IsDataExpanded;
                SampleTemplateTestData.LevelZeroChildren[1].IsLevelOneExpanded = !SampleTemplateTestData.LevelZeroChildren[1].IsLevelOneExpanded;
            }
            else
            {
                //var node = ((TestNode)SampleData).Children[1];

                //if (node.Children.Count == 3)
                //{
                //    _spareNode = node.Children[1];
                //    node.Children.RemoveAt(1);
                //}
                //else
                //{
                //    node.Children.Insert(1, _spareNode);
                //}
            }

            {
                //var node = SampleTemplateTestData.LevelZeroChildren[1];

                //if (node.LevelOneChildren.Count == 3)
                //{
                //    _spareTemplateNode = node.LevelOneChildren[1];
                //    node.LevelOneChildren.RemoveAt(1);
                //}
                //else
                //{
                //    node.LevelOneChildren.Insert(1, _spareTemplateNode);
                //}
            }

            Count++;

            return true;
        }

        public object SampleData { get; }
        public IList SampleListData { get; }
        public LevelZero SampleTemplateTestData { get; }

        private object GetSampleTree()
        {
            var root = new TestNode("Root");
            var child0 = new TestNode("0");
            var child1 = new TestNode("1");
            var child2 = new TestNode("2");

            root.IsDataExpanded = true;

            root.Children.Add(child0);
            root.Children.Add(child1);
            root.Children.Add(child2);

            child1.IsDataExpanded = true;

            new TestNode("0-0").Parent = child0;
            new TestNode("0-1").Parent = child0;
            new TestNode("0-2").Parent = child0;

            new TestNode("1-0").Parent = child1;
            new TestNode("1-1").Parent = child1;
            new TestNode("1-2").Parent = child1;

            new TestNode("2-0").Parent = child2;
            new TestNode("2-1").Parent = child2;
            new TestNode("2-2").Parent = child2;

            return root;

            var retval = new List<object>();
            retval.Add(root);
            return retval;
        }


        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(SelectedItem))
            {
                Debug.WriteLine($"SelectedItem:{(SelectedItem?.Name ?? "null")}");
            }
        }
    }
}

