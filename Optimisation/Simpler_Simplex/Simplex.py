import json
from fractions import Fraction
import copy


class Simplex:
    def __init__(self):
        self.variable = []
        self.ecart = []
        self.artificiel = []
        self.base = {}
        self.matrice = []
        self.dimVar = None
        self.dimCont = None
        self.type = None
        self.place = {}

    def readFile(self, path="Data.json"):
        with open(path, 'r') as file:
            json_data = json.load(file)
        self.type = int(json_data["type"])
        self.dimVar = int(json_data["dimVar"])
        self.dimCont = int(json_data["dimCont"])
        self.variable = json_data["variable"]!
        self.ecart = json_data["contrainte"]
        self.artificiel = json_data["artificiel"]
        for indice, value in enumerate(json_data["base"]):
            self.base[indice] = value
        for indice, value in enumerate(json_data["place"]):
            self.place[indice] = value
        for i in range(len(json_data["matrice"])):
            tab = []
            for j in range(len(json_data["matrice"][i])):
                tab.append(Fraction(json_data["matrice"][i][j]))
            self.matrice.append(tab)
        self.showTab()

    def showTab(self):
        for y in range(len(self.matrice)):
            for x in range(len(self.matrice[y])):
                print(str(self.matrice[y][x]) + " ", end='')
            print(" ")

    def isThereAnyPositive(self):
        little = 0
        isZero = False
        for number in range(len(self.matrice[-1]) - 1):
            if self.matrice[-1][number] > 0:
                if self.matrice[-1][number] > self.matrice[-1][little]:
                    little = number
                isZero = True
        if isZero == False:
            return -1
        return little

    def isThereAnyNegative(self):
        little = float('inf')
        index = -1
        for i, value in enumerate(self.matrice[-1]):
            if value < 0 and value < little and i<(len(self.matrice[-1])-1):
                little = value
                index = i
        return index

    def getMin(self, indice):
        kely = 0
        min_row = float('inf')
        for i in range(len(self.matrice) - 1):
            if self.matrice[i][indice] > 0:
                if 0 < self.matrice[i][-1] / self.matrice[i][indice] < min_row:
                    min_row = self.matrice[i][-1] / self.matrice[i][indice]
                    kely = i
        if min_row == float('inf'):
            return -1
        return kely

    def swapBase(self, min, max):
        indexVar = self.variable.index(self.place[max])
        temp = self.variable[indexVar]
        self.variable[indexVar] = self.base[min]
        self.base[min] = copy.copy(temp)

    def setPivotTo1(self, minimum, maximum):
        self.swapBase(minimum, maximum)
        intersection = self.matrice[minimum][maximum]
        for i in range(len(self.matrice[minimum])):
            self.matrice[minimum][i] = self.matrice[minimum][i] / intersection

    def Gauss(self, min, max):
        for i in range(len(self.matrice)):
            coeff = self.matrice[i][max]
            if i != min:
                for j in range(len(self.matrice[min])):
                    self.matrice[i][j] = self.matrice[i][j] - (coeff * self.matrice[min][j])

    def Maximize(self):
        maximum = self.isThereAnyPositive()
        if maximum == -1:
            return self.matrice[-1][-1]
        minus = self.getMin(maximum)
        if minus == -1:
            raise Exception("Pas de solution")
        self.setPivotTo1(minus, maximum)
        self.Gauss(minus, maximum)
        return self.Maximize()

    def Minimize(self):
        maximumNeg=self.isThereAnyNegative()
        print("maximum=",maximumNeg)
        if maximumNeg==-1:
            return self.matrice[-1][-1]
        minus=self.getMin(maximumNeg)
        print("minimum=",minus)
        if minus==-1:
            raise Exception("Pas de solution")
        self.setPivotTo1(minus, maximumNeg)
        self.Gauss(minus, maximumNeg)
        return self.Minimize()

    def firstPhase(self):
        minimisation=self.Minimize()
        self.showTab()
        if minimisation!=0:
            raise Exception("Pas de solution")
        else:
            self.artificiel=[]


    def Resolve(self):
        self.readFile()
        if len(self.artificiel)>0:
            self.firstPhase()
        else:
            if self.type==0:
                return self.Maximize()
            elif self.type==1:
                return self.Minimize()
