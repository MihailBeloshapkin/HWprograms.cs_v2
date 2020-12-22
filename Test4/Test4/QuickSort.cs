using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test4
{
	/// <summary>
	/// This class contains diffrent realizations 
	/// of the QuickSort method.
	/// </summary>
	public class QuickSort
	{

		/// <summary>
		/// Simple realization.
		/// </summary>
		public static void SimpleQuickSort(int[] array, int head, int tail)
		{
			int indexOfElement = (head + tail) / 2;
			int indexOfHead = head - 1;
			int indexOfTail = tail + 1;

			if (head >= tail)
			{
				return;
			}
			while (indexOfHead < indexOfTail)
			{
				indexOfHead++;
				while (array[indexOfHead] < array[indexOfElement])
				{
					indexOfHead++;
				}
				indexOfTail--;
				while (array[indexOfTail] > array[indexOfElement])
				{
					indexOfTail--;
				}

				if (indexOfHead < indexOfTail)
				{
					int tmp = array[indexOfTail];
					array[indexOfTail] = array[indexOfHead];
					array[indexOfHead] = tmp;

					if (indexOfElement == indexOfHead)
					{
						indexOfElement = indexOfTail;
						indexOfTail++;
					}
					if (indexOfElement == indexOfTail)
					{
						indexOfElement = indexOfHead;
						indexOfHead--;
					}
				}
			}

			SimpleQuickSort(array, head, indexOfElement - 1);
			SimpleQuickSort(array, indexOfElement + 1, tail);
		}

		/// <summary>
		/// Parallel Quick Sort realization
		/// </summary>
		public static void ParallelQuickSort(int[] array, int head, int tail)
		{
			if (tail - head < 2000)
			{
				SimpleQuickSort(array, head, tail);
				return;
			}

			int indexOfElement = (head + tail) / 2;
			int indexOfHead = head - 1;
			int indexOfTail = tail + 1;

			if (head >= tail)
			{
				return;
			}
			while (indexOfHead < indexOfTail)
			{
				indexOfHead++;
				while (array[indexOfHead] < array[indexOfElement])
				{
					indexOfHead++;
				}
				indexOfTail--;
				while (array[indexOfTail] > array[indexOfElement])
				{
					indexOfTail--;
				}

				if (indexOfHead < indexOfTail)
				{
					int tmp = array[indexOfTail];
					array[indexOfTail] = array[indexOfHead];
					array[indexOfHead] = tmp;

					if (indexOfElement == indexOfHead)
					{
						indexOfElement = indexOfTail;
						indexOfTail++;
					}
					if (indexOfElement == indexOfTail)
					{
						indexOfElement = indexOfHead;
						indexOfHead--;
					}
				}
			}

			Parallel.Invoke(
				() => ParallelQuickSort(array, head, indexOfElement - 1),
				() => ParallelQuickSort(array, indexOfElement + 1, tail));

		}
	}
}
