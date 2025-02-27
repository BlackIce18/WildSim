﻿using System.Collections.Generic;
using System;
using System.IO;
using System.Diagnostics;

/*
 код типа IComparable , это позволит нам сортировать его напрямую при обращении к нему. В качестве значения, по которому сортируется сеть, используется пригодность сети.
 */
public class NeuralNetwork : IComparable<NeuralNetwork>
{
    private int[] _layers;
    private float[][] _neurons;
    private float[][] _biases;
    private float[][][] _weights;
    private int[] _activations;
    public string biases = "";
    public float fitness = 0;

    public NeuralNetwork(int[] layers)
    {
        _layers = new int[layers.Length];
        for (int i = 0; i < layers.Length; i++)
        {
            _layers[i] = layers[i];
        }
        InitNeurons();
        InitBiases();
        InitWeights();
    }

    private void InitNeurons()//создайте пустой массив памяти для нейронов в сети.
    {
        List<float[]> neuronsList = new List<float[]>();
        for (int i = 0; i < _layers.Length; i++)
        {
            neuronsList.Add(new float[_layers[i]]);
        }
        _neurons = neuronsList.ToArray();
    }

    private void InitBiases()//инициализирует и заполняет массив для смещений, хранящихся в сети
    {
        List<float[]> biasList = new List<float[]>();
        biases = "[";
        for (int i = 0; i < _layers.Length; i++)
        {
            float[] bias = new float[_layers[i]];
            for (int j = 0; j < _layers[i]; j++)
            {
                bias[j] = UnityEngine.Random.Range(-0.5f, 0.5f);
                biases += i+" "+j+")["+bias[j] + "] ";
            }

            biasList.Add(bias);
        }
        biases += "]";
        _biases = biasList.ToArray();
    }

    private void InitWeights()//инициализирует случайный массив для весов, хранящихся в сети.
    {
        List<float[][]> weightsList = new List<float[][]>();
        for (int i = 1; i < _layers.Length; i++)
        {
            List<float[]> layerWeightsList = new List<float[]>();
            int neuronsInPreviousLayer = _layers[i - 1];
            for (int j = 0; j < _neurons[i].Length; j++)
            {
                float[] neuronWeights = new float[neuronsInPreviousLayer];
                for (int k = 0; k < neuronsInPreviousLayer; k++)
                {
                    //float sd = 1f / ((_neurons[i].Length + neuronsInPreviousLayer) / 2f);
                    neuronWeights[k] = UnityEngine.Random.Range(-0.5f, 0.5f);
                }
                layerWeightsList.Add(neuronWeights);
            }
            weightsList.Add(layerWeightsList.ToArray());
        }
        _weights = weightsList.ToArray();
    }

    public float[] FeedForward(float[] inputs)//Прямое распространение, вход >==> выход.
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            _neurons[0][i] = inputs[i];
        }
        for (int i = 1; i < _layers.Length; i++)
        {
            int layer = i - 1;
            for (int j = 0; j < _neurons[i].Length; j++)
            {
                float value = 0f;
                for (int k = 0; k < _neurons[i - 1].Length; k++)
                {
                    value += _weights[i - 1][j][k] * _neurons[i - 1][k];
                }
                _neurons[i][j] = Activate(value + _biases[i][j]);
            }
        }
        return _neurons[_neurons.Length - 1];
    }

    public float Activate(float value)
    {
        return (float)Math.Tanh(value);
    }

    public void Mutate(int chance, float val) //Функция мутации
    {
        for (int i = 0; i < _biases.Length; i++)
        {
            for (int j = 0; j < _biases[i].Length; j++)
            {
                _biases[i][j] = (UnityEngine.Random.Range(0f, chance) <= 5) ? _biases[i][j] += UnityEngine.Random.Range(-val, val) : _biases[i][j];
            }
        }

        for (int i = 0; i < _weights.Length; i++)
        {
            for (int j = 0; j < _weights[i].Length; j++)
            {
                for (int k = 0; k < _weights[i][j].Length; k++)
                {
                    _weights[i][j][k] = (UnityEngine.Random.Range(0f, chance) <= 5) ? _weights[i][j][k] += UnityEngine.Random.Range(-val, val) : _weights[i][j][k];
                }
            }
        }
    }

    public int CompareTo(NeuralNetwork other) //Сравнение производительности Нейронных сетей
    {
        if (other == null) return 1;

        if (fitness > other.fitness)
            return 1;
        else if (fitness < other.fitness)
            return -1;
        else
            return 0;
    }

    public NeuralNetwork copy(NeuralNetwork nn) //Для создания глубокой копии, чтобы обеспечить сериализацию массивов.
    {
        for (int i = 0; i < _biases.Length; i++)
        {
            for (int j = 0; j < _biases[i].Length; j++)
            {
                nn._biases[i][j] = _biases[i][j];
            }
        }
        for (int i = 0; i < _weights.Length; i++)
        {
            for (int j = 0; j < _weights[i].Length; j++)
            {
                for (int k = 0; k < _weights[i][j].Length; k++)
                {
                    nn._weights[i][j][k] = _weights[i][j][k];
                }
            }
        }
        return nn;
    }

    public void Load(string path) //Загружает смещения и веса из файла в нейронную сеть.
    { 
        TextReader tr = new StreamReader(path);
        int NumberOfLines = (int)new FileInfo(path).Length;
        string[] ListLines = new string[NumberOfLines];
        int index = 1;
        for (int i = 1; i < NumberOfLines; i++)
        {
            ListLines[i] = tr.ReadLine();
        }
        tr.Close();
        if (new FileInfo(path).Length > 0)
        {
            for (int i = 0; i < _biases.Length; i++)
            {
                for (int j = 0; j < _biases[i].Length; j++)
                {
                    _biases[i][j] = float.Parse(ListLines[index]);
                    index++;
                }
            }

            for (int i = 0; i < _weights.Length; i++)
            {
                for (int j = 0; j < _weights[i].Length; j++)
                {
                    for (int k = 0; k < _weights[i][j].Length; k++)
                    {
                        _weights[i][j][k] = float.Parse(ListLines[index]);
                        index++;
                    }
                }
            }
        }
    }

    public void Save(string path) //Используется для сохранения смещений и весов в сети в файл
    {
        File.Create(path).Close();
        StreamWriter writer = new StreamWriter(path, true);

        for (int i = 0; i < _biases.Length; i++)
        {
            for (int j = 0; j < _biases[i].Length; j++)
            {
                writer.WriteLine(_biases[i][j]);
            }
        }

        for (int i = 0; i < _weights.Length; i++)
        {
            for (int j = 0; j < _weights[i].Length; j++)
            {
                for (int k = 0; k < _weights[i][j].Length; k++)
                {
                    writer.WriteLine(_weights[i][j][k]);
                }
            }
        }
        writer.Close();
    }
}
