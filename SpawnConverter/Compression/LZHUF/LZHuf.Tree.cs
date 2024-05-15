using System;

namespace SpawnConverter.Compression
{
    public partial class LZHuf
    {
        /* Инициализация дерева частот */
        private static void StartHuff()
        {
            int i, j;

            for (i = 0; i < N_CHAR; i++)
            {
                freq[i] = 1;
                son[i] = i + T;
                prnt[i + T] = i;
            }

            i = 0; j = N_CHAR;

            while (j <= R)
            {
                freq[j] = freq[i] + freq[i + 1];
                son[j] = i;
                prnt[i] = prnt[i + 1] = j;
                i += 2; j++;
            }

            freq[T] = 0xffff;
            prnt[R] = 0;
        }

        /* Реорганизация дерева частот */
        private static void Reconst()
        {
            int i, k, j = 0;

            for (i = 0; i < T; i++)
            {
                if (son[i] >= T)
                {
                    freq[j] = (freq[i] + 1) / 2;
                    son[j] = son[i];
                    j++;
                }
            }

            for (i = 0, j = N_CHAR; j < T; i += 2, j++)
            {
                k = i + 1;
                uint f = freq[j] = freq[i] + freq[k];

                for (k = j - 1; f < freq[k]; k--) ;
                k++;

                int l = (j - k) * sizeof(uint);
                
                Buffer.BlockCopy(freq, sizeof(uint) * k, freq, sizeof(uint) * (k + 1), l);
                freq[k] = f;

                Buffer.BlockCopy(son, sizeof(uint) * k, son, sizeof(uint) * (k + 1), l);
                son[k] = i;
            }

            for (i = 0; i < T; i++)
            {
                if ((k = son[i]) >= T)
                {
                    prnt[k] = i;
                }
                else
                {
                    prnt[k] = prnt[k + 1] = i;
                }
            }
        }

        //---------------------------------------------------------
        /* Обновление дерева частот */
        private static void Update(int c)
        {
            if (freq[R] == MAX_FREQ)
            {
                Reconst();
            }

            c = prnt[c + T];

            do
            {
                uint k = ++freq[c];

                int l = c + 1;

                if (k > freq[l])
                {
                    while (k > freq[++l]) ;
                    l--;

                    freq[c] = freq[l];
                    freq[l] = k;

                    int i = son[c];

                    prnt[i] = l;

                    if (i < T)
                        prnt[i + 1] = l;

                    int j = son[l];
                    son[l] = i;

                    prnt[j] = c;

                    if (j < T)
                        prnt[j + 1] = c;

                    son[c] = j;

                    c = l;
                }
            }
            while ((c = prnt[c]) != 0);
        }
    }
}
