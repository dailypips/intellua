﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intellua
{
    class IntelluaSource
    {
        bool m_copy;
        public int pos;
        public string text {
            get { 
                if(m_copy) return m_text;
                return m_intellua.Text;
            }
        }
        public Byte[] RawText {
            get {
                if (m_copy) return rawText;
                return m_intellua.RawText;
            }
        }
        public string FilePath {
            get {
                return m_filepath;
            }
        }
        string m_text;
        string m_filepath;
        Byte[] styledText;
        Byte[] rawText;
        public Intellua m_intellua;

        public IntelluaSource(string filename,Intellua parent) {
            m_copy = true;
            pos = 0;
            m_intellua = new Intellua();
            m_intellua.Parse = false;
            m_intellua.AutoCompleteData.setParent(parent.AutoCompleteData.getParent());
            m_intellua.Text = System.IO.File.ReadAllText(filename);
            m_intellua.FilePath = filename;
            m_filepath = m_intellua.FilePath;

            m_text = m_intellua.Text;
            rawText = m_intellua.RawText;
            ScintillaNET.Range range = new ScintillaNET.Range(0, rawText.Length, m_intellua);
            styledText = range.StyledText;
        }

        public IntelluaSource(Intellua intellua,bool copy = false) {
            m_copy = copy;
            pos = intellua.CurrentPos;
            m_intellua = intellua;
            m_filepath = m_intellua.FilePath;
            if (copy)
            {
                m_text =intellua.Text;
                rawText = intellua.RawText;
                ScintillaNET.Range range = new ScintillaNET.Range(0, rawText.Length, intellua);
                styledText = range.StyledText;
            }
        }

        public int getStyleAt(int p) {
            if (m_copy)
                return styledText[p * 2 + 1] & 0x1f;
            else
                return m_intellua.Styles.GetStyleAt(p) & 0x1f;
        }

        public int getRawPos(int p = -1) {
            if (p == -1) {
                if (m_copy)
                {
                    return pos;
                }
                else {
                    return m_intellua.CurrentPos;
                }
            }
            return Encoding.UTF8.GetByteCount(text.ToCharArray(), 0, p + 1) - 1;
        }

        public int getDecodedPos(int p  = -1) {
            if (p == -1)
            {
                if (m_copy)
                    p = pos;
                else p = m_intellua.CurrentPos;

            }
            if (m_copy)
            {
                return Encoding.UTF8.GetCharCount(rawText, 0, p + 1) - 1;
            }
            else {
                return Encoding.UTF8.GetCharCount(m_intellua.RawText, 0, p + 1) - 1;
            }
        }
    }
}
