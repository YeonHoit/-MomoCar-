export const priceComma = (target: string): string => {
  // let num: number = parseInt(target, 10)
  // console.log(num.toLocaleString('ko-KR'))
  // return num.toLocaleString('ko-KR')
  // return target.replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ',')
  let len, point, str

  point = target.length % 3
  len = target.length

  str = target.substring(0, point)
  while (point < len) {
    if (str != '') str += ','
    str += target.substring(point, point + 3)
    point += 3
  }
  return str
}
