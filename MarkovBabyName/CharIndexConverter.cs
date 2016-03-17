using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkovBabyName
{
    public class CharIndexConverter
    {
        public Dictionary<char, int> Mapping { get; set; }

        public CharIndexConverter()
        {
            Mapping = new Dictionary<char, int>();
            Mapping.Add('a', 0);
            Mapping.Add('b', 1);
            Mapping.Add('c', 2);
            Mapping.Add('d', 3);
            Mapping.Add('e', 4);
            Mapping.Add('f', 5);
            Mapping.Add('g', 6);
            Mapping.Add('h', 7);
            Mapping.Add('i', 8);
            Mapping.Add('j', 9);
            Mapping.Add('k', 10);
            Mapping.Add('l', 11);
            Mapping.Add('m', 12);
            Mapping.Add('n', 13);
            Mapping.Add('o', 14);
            Mapping.Add('p', 15);
            Mapping.Add('q', 16);
            Mapping.Add('r', 17);
            Mapping.Add('s', 18);
            Mapping.Add('t', 19);
            Mapping.Add('u', 20);
            Mapping.Add('v', 21);
            Mapping.Add('w', 22);
            Mapping.Add('x', 23);
            Mapping.Add('y', 24);
            Mapping.Add('z', 25);
            Mapping.Add('_', 26);
        }

        public int IndexFromChar(char c)
        {
            int record;
            Mapping.TryGetValue(char.ToLower(c), out record);
            return record;
        }

        public char CharFromIndex(int c)
        {
            return Mapping.FirstOrDefault(x => x.Value == c).Key;

        }
    }
}