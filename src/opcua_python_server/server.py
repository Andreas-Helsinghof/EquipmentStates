import asyncio
import logging
import random
from asyncua import Server, ua


async def main():
    _logger = logging.getLogger("asyncua")
    # setup our server
    server = Server()
    await server.init()
    server.set_endpoint("opc.tcp://0.0.0.0:4840/freeopcua/server/")

    # setup our own namespace, not really necessary but should as spec
    uri = "http://examples.freeopcua.github.io"
    idx = await server.register_namespace(uri)
    ns = "ns=2;s=freeopcua.Tags.pressure"
    ns3 = "ns=2;s=freeopcua.Tags.Setpressure"
    ns2 = "ns=2;s=freeopcua.Tags.temperature"

    min_val = 0
    max_val = 10

    # populating our address space
    # server.nodes, contains links to very common nodes like objects and root
    myobj = await server.nodes.objects.add_object(idx, "MyObject")
    pressure = await myobj.add_variable(ns, "MyVariable", 10.5)
    temperature = await myobj.add_variable(ns2, "MyVariable", 26.7)
    # Set MyVariable to be writable by clients
    await pressure.set_writable()
    await temperature.set_writable()
    opcs = [pressure, temperature]

    # Add method to update pressure
    async def set_pressure(value):
       # _logger.info(f"SetPressure method called with value: {value}")
        await pressure.write_value(float(value))
        return True

    await myobj.add_method(ns3, "SetPressure", set_pressure, [ua.VariantType.Double])

    _logger.info("Starting server!")
    async with server:
        while True:
            await asyncio.sleep(2)
            random_counter = random.uniform(min_val, max_val)
            for opc in opcs:
                _logger.info("Set value of %s to %.1f", opc, random_counter)
                await opc.write_value(random_counter)


if __name__ == "__main__":
    logging.basicConfig(level=logging.INFO)
    asyncio.run(main(), debug=True)
