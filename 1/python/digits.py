import pathlib
import sys


digit_names = [
    "zero",
    "one",
    "two",
    "three",
    "four",
    "five",
    "six",
    "seven",
    "eight",
    "nine",
]


class Calibration:
    def __init__(self, line):
        # A single line from the calibration file.
        self.line = line
        # Digits which appear first and last
        self.first = sys.maxsize
        self.final = -1
        # Indices of the first and final digits found so far.
        self.first_index = sys.maxsize
        self.final_index = -1

        # Find the first and final occurrence of each digit 1..9
        # and hence track the first and final digit overall.
        for digit in range(1, 10):
            index = self.find_first(digit)
            if index != -1 and index < self.first_index:
                self.first = digit
                self.first_index = index
            index = self.find_final(digit)
            if index != -1 and index > self.final_index:
                self.final = digit
                self.final_index = index

        # Combine the first and final digits into a two-digit number.
        self.result = (self.first * 10) + self.final

    def find_first(self, digit):
        """
        Find first occurrence of the specified digit, considering
        numeric and verbal instances, e.g. both "7" and "seven".
        """
        text_to_find = str(digit)
        first_digit = self.line.find(text_to_find)
        text_to_find = digit_names[digit]
        first_name = self.line.find(text_to_find)
        # Two binary flags give four possible scenarios
        if first_digit == -1 and first_name == -1:
            # 0 0 - both missing
            first = -1
        elif first_digit == -1 and first_name != -1:
            # 0 1 - name but no digit
            first = first_name
        elif first_digit != -1 and first_name == -1:
            # 1 0 - digit but no name
            first = first_digit
        else:
            # 1 1 - both present - choose first one
            first = first_digit if first_digit < first_name else first_name
        return first

    def find_final(self, digit):
        """
        Find final occurrence of the specified digit, considering
        numeric and verbal instances, e.g. both "6" and "six".
        """
        text_to_find = str(digit)
        final_digit = self.line.rfind(text_to_find)
        text_to_find = digit_names[digit]
        final_name = self.line.rfind(text_to_find)
        # Two binary flags give four possible scenarios
        if final_digit == -1 and final_name == -1:
            # 0 0 - both missing
            final = -1
        elif final_digit == -1 and final_name != -1:
            # 0 1 - name but no digit
            final = final_name
        elif final_digit != -1 and final_name == -1:
            # 1 0 - digit but no name
            final = final_digit
        else:
            # 1 1 - both present - choose later one
            final = final_digit if final_digit > final_name else final_name
        return final


script_dir = pathlib.Path(__file__).parent
input_path = script_dir.parent / "input.txt"
total = 0
lines = 0
lines_without_digits = 0
with input_path.open() as file:
    while line := file.readline().strip():
        lines += 1
        cal = Calibration(line)
        if cal.result > 0 and cal.result < 100:
            # It's valid
            total += cal.result
            # Uncomment the next line for verbose output.
            # print(f"{lines}: {cal.result}")
        else:
            lines_without_digits += 1

print(f"{lines} lines processed ({lines_without_digits} without digits)")
print(f"Total: {total}")
