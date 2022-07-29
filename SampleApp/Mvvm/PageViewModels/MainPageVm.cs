﻿using SampleApp.Mvvm.ViewModels;
using SampleApp.Mvvm.ViewModels.TemplateTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp.Mvvm.PageViewModels
{
    public class MainPageVm : BaseVm
    {
        private TestNode _spareNode;
        private LevelTwo _spareTemplateNode;

        public MainPageVm()
        {
            SampleData = GetSampleTree();

            SampleTemplateTestData = new LevelZero("Root") { IsLevelZeroExpanded = true };

            //Device.StartTimer(TimeSpan.FromMilliseconds(300), Tick);
        }

        private int _count;
        private bool _isRootVisible;

        public int Count { get => _count; set => SetProperty(ref _count, value); }
        public bool IsRootVisible { get => _isRootVisible; set => SetProperty(ref _isRootVisible, value); }

        private bool Tick()
        {
            IsRootVisible = (Count & 8)==0;
            if ((Count % 8) < 2)
            {
                Device.BeginInvokeOnMainThread(() => ((TestNode)SampleData).IsDataExpanded = !((TestNode)SampleData).IsDataExpanded);
                Device.BeginInvokeOnMainThread(() => SampleTemplateTestData.IsLevelZeroExpanded = !SampleTemplateTestData.IsLevelZeroExpanded);
            }

            if ((Count % 3) != 0)
            {
                Device.BeginInvokeOnMainThread(() => ((TestNode)SampleData).Children[1].IsDataExpanded = !((TestNode)SampleData).Children[1].IsDataExpanded);
                Device.BeginInvokeOnMainThread(() => SampleTemplateTestData.LevelZeroChildren[1].IsLevelOneExpanded = !SampleTemplateTestData.LevelZeroChildren[1].IsLevelOneExpanded);
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var node = ((TestNode)SampleData).Children[1];

                    if (node.Children.Count == 3)
                    {
                        _spareNode = node.Children[1];
                        node.Children.RemoveAt(1);
                    }
                    else
                    {
                        node.Children.Insert(1, _spareNode);
                    }

                }
                );

                Device.BeginInvokeOnMainThread(() =>
                {
                    var node = SampleTemplateTestData.LevelZeroChildren[1];

                    if (node.LevelOneChildren.Count == 3)
                    {
                        _spareTemplateNode = node.LevelOneChildren[1];
                        node.LevelOneChildren.RemoveAt(1);
                    }
                    else
                    {
                        node.LevelOneChildren.Insert(1, _spareTemplateNode);
                    }

                }
                );
            }
            Count++;

            return true;
        }

        public object SampleData { get; }
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
    }
}

